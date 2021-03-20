using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.DataBlocks.Symbols;
using QRCodeBaseLib.DataBlocks.Symbols.EncodingSymbols;
using QRCodeDiag.UserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace QRCodeDiag
{
    internal class SettingsPropertyManager
    {
        public delegate void SettingsPropertyChangedEventHandler();
        public event SettingsPropertyChangedEventHandler PropertyChangedEvent;
        private enum PropertyType
        {
            VersionNumber,
            ECCLevel,
            EncodingType,
            MessageContent,
            MessageLength,
            MaskType,
            ///////////
            RawCode,
            RawDataBytes,
            RawECCBytes,
            MessageModeSymbol,
            PaddingBytes,
            EncodedSymbols,
            TerminatorSymbol
        }

        private QRCode qrCode;
        private readonly DrawingManager drawingManager;
        private readonly Dictionary<PropertyType, CodeSymbolCodeOptionsItem> settingsControls;
        private readonly Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> drawingProperties;
        private readonly ControlCollection ctrlCollection;

        public SettingsPropertyManager(DrawingManager setDrawingManager, ControlCollection setCtrlCollection)
        {
            this.drawingManager = setDrawingManager;
            this.settingsControls = new Dictionary<PropertyType, CodeSymbolCodeOptionsItem>();
            this.drawingProperties = CreateDrawingProperties();
            this.ctrlCollection = setCtrlCollection;
        }

        private static Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> CreateDrawingProperties()
        {
            return new Dictionary<PropertyType, CodeSymbolCodeDrawingProperties>
            {
                [PropertyType.RawCode]           = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Orange,     Color.Orange,     Color.Orange    ), Color.Orange,    "Raw Code"           ),
                [PropertyType.RawDataBytes]      = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.DarkOrange, Color.DarkOrange, Color.DarkOrange), Color.LightBlue, "Raw Data Bytes"     ),
                [PropertyType.RawECCBytes]       = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Orange,     Color.Orange,     Color.Orange    ), Color.LightBlue, "Raw ECC Bytes"      ),
                [PropertyType.MessageModeSymbol] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Blue,       Color.LightBlue,  Color.DarkCyan  ), Color.LightBlue, "Message Mode Symbol"),
                [PropertyType.PaddingBytes]      = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Blue,       Color.LightBlue,  Color.DarkCyan  ), Color.LightBlue, "Padding Bytes"      ),
                [PropertyType.EncodedSymbols]    = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Red,        Color.LightBlue,  Color.Orange    ), Color.LightBlue, "Encoded Symbols"    ),
            };
        }

        #region private methods
        private void UnregisterEventHandlers()
        {
            qrCode.RawCodeChangedEvent -= HandleRawCodeChanged;
            qrCode.RawDataBytesChangedEvent -= HandleRawDataBytesChanged;
            qrCode.RawECCBytesChangedEvent -= HandleRawECCBytesChanged;
            qrCode.MessageModeChangedEvent -= HandleMessageModeChanged;
            qrCode.PaddingBytesChangedEvent -= HandlePaddingBytesChanged;
            qrCode.EncodedSymbolsChangedEvent -= HandleEncodedSymbolsChanged;
            qrCode.TerminatorSymbolChangedEvent -= HandleTerminatorSymbolChanged;
        }
        private void RegisterEventHandlers()
        {
            qrCode.RawCodeChangedEvent += HandleRawCodeChanged;
            qrCode.RawDataBytesChangedEvent += HandleRawDataBytesChanged;
            qrCode.RawECCBytesChangedEvent += HandleRawECCBytesChanged;
            qrCode.MessageModeChangedEvent += HandleMessageModeChanged;
            qrCode.PaddingBytesChangedEvent += HandlePaddingBytesChanged;
            qrCode.EncodedSymbolsChangedEvent += HandleEncodedSymbolsChanged;
            qrCode.TerminatorSymbolChangedEvent += HandleTerminatorSymbolChanged;
        }
        //private void RegisterMessageContentListener()
        //{
        //    var messageOptionsItem = new StringValueOptionsItem("Message");

        //    messageOptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();   // ToDo write only message value in message field or adapt length indicator (maybe different length indicator allowed to write only partial messages)? (re)create ECC for message?
        //    this.settingsControls[SettingsProperties.MessageContent]= messageOptionsItem;
        //    this.qrCode.MessageChangedEvent += (msg, valid) => messageOptionsItem.StringValue = msg; // ToDo inform the user if the message is valid or an error message is displayed
        //}

        /// <summary>
        /// If a Control exists for the given <paramref name="ctrlType"/> this function removes it
        /// from <cref>ctrlCollection</cref> and <cref>settingsControls</cref>.
        /// The associated <cref>DrawableCodeSymbolCode</cref> is also removed from <cref>drawingManager</cref>
        /// </summary>
        /// <param name="ctrlType">Type of control to be removed</param>
        private void RemoveControlType(PropertyType ctrlType)
        {
            if (this.settingsControls.TryGetValue(ctrlType, out var oldOptionsItem))
            {
                oldOptionsItem.PropertyChangedEvent -= this.PropertyChangedEvent;
                this.drawingManager.UnregisterCodeSymbolCode(oldOptionsItem.DrawableCodeSymbolCode);

                if (this.ctrlCollection.Contains(oldOptionsItem))
                {
                    this.ctrlCollection.Remove(oldOptionsItem);
                }

                this.settingsControls.Remove(ctrlType);
            }
        }

        private void RemoveAllControlTypes()
        {
            foreach (PropertyType pType in Enum.GetValues(typeof(PropertyType)))
            {
                RemoveControlType(pType);
            }
        }

        private void AddCodeSymbolCodeOptionsItem(DrawableCodeSymbolCode drawableCode, PropertyType ctrlType)
        {
            var newOptionsItem = new CodeSymbolCodeOptionsItem(drawableCode.DisplayName, drawableCode);

            newOptionsItem.PropertyChangedEvent += this.PropertyChangedEvent;
            this.ctrlCollection.Add(newOptionsItem);
            this.drawingManager.RegisterCodeSymbolCode(drawableCode);
            this.settingsControls[ctrlType] = newOptionsItem;
        }
        private void HandleCodeChange(ICodeSymbolCode codeSymbolCode, PropertyType propertyType)
        {
            var drawProps = this.drawingProperties[propertyType];

            this.RemoveControlType(propertyType);

            if (codeSymbolCode != null)
            {
                var drawableCode = new DrawableCodeSymbolCode(codeSymbolCode, drawProps);
                this.AddCodeSymbolCodeOptionsItem(drawableCode, propertyType);
            }
        }
        private void HandleRawCodeChanged(CodeSymbolCode<RawCodeByte> newRawCode)
        {
            this.HandleCodeChange(newRawCode, PropertyType.RawCode);
        }
        private void HandleRawDataBytesChanged(CodeSymbolCode<RawCodeByte> newRawDataBytes)
        {
            this.HandleCodeChange(newRawDataBytes, PropertyType.RawDataBytes);
        }
        private void HandleRawECCBytesChanged(CodeSymbolCode<RawCodeByte> newRawECCBytes)
        {
            this.HandleCodeChange(newRawECCBytes, PropertyType.RawECCBytes);
        }
        private void HandleMessageModeChanged(CodeSymbolCode<MessageModeSymbol> newMessageModeSymbol)
        {
            this.HandleCodeChange(newMessageModeSymbol, PropertyType.MessageModeSymbol);
        }
        private void HandlePaddingBytesChanged(CodeSymbolCode<RawCodeByte> newPaddingBytes)
        {
            this.HandleCodeChange(newPaddingBytes, PropertyType.PaddingBytes);
        }
        private void HandleEncodedSymbolsChanged(CodeSymbolCode<ByteEncodingSymbol> newEncodedSymbols)
        {
            this.HandleCodeChange(newEncodedSymbols, PropertyType.EncodedSymbols);
        }
        private void HandleTerminatorSymbolChanged(TerminatorSymbol newTerminatorSymbol)
        {

        }
        #endregion
        #region public methods
        public void SetQRCode(QRCode newQRCode)
        {
            if (this.qrCode != null)
            {
                this.UnregisterEventHandlers();
                this.RemoveAllControlTypes();
            }

            this.qrCode = newQRCode;

            if(this.qrCode != null)
                this.RegisterEventHandlers();
        }
        #endregion
    }
}
