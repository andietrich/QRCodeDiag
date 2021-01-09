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
        private readonly ControlCollection ctrlCollection;

        public SettingsPropertyManager(DrawingManager setDrawingManager, ControlCollection setCtrlCollection)
        {
            this.drawingManager = setDrawingManager;
            this.settingsControls = new Dictionary<PropertyType, CodeSymbolCodeOptionsItem>();
            this.ctrlCollection = setCtrlCollection;
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

        private void AddCodeSymbolCodeOptionsItem(string codeSymbolName, DrawableCodeSymbolCode drawableCode, PropertyType ctrlType)
        {
            var newOptionsItem = new CodeSymbolCodeOptionsItem(codeSymbolName, drawableCode);

            newOptionsItem.PropertyChangedEvent += this.PropertyChangedEvent;
            this.ctrlCollection.Add(newOptionsItem);
            this.drawingManager.RegisterCodeSymbolCode(drawableCode);
            this.settingsControls[ctrlType] = newOptionsItem;
        }

        private void HandleRawCodeChanged(CodeSymbolCode<RawCodeByte> newRawCode)
        {
            var propType = PropertyType.RawCode;

            this.RemoveControlType(propType);

            if (newRawCode != null)
            {
                var symbolColors = new SymbolColors(Color.Orange, Color.Orange, Color.Orange);
                var drawableCode = new DrawableCodeSymbolCode(newRawCode, symbolColors, Color.Orange);
                this.AddCodeSymbolCodeOptionsItem("Raw Code", drawableCode, propType);
            }
        }
        private void HandleRawDataBytesChanged(CodeSymbolCode<RawCodeByte> newRawDataBytes)
        {
            var propType = PropertyType.RawDataBytes;

            this.RemoveControlType(propType);

            if (newRawDataBytes != null)
            {
                var symbolColors = new SymbolColors(Color.DarkOrange, Color.DarkOrange, Color.DarkOrange);
                var drawableCode = new DrawableCodeSymbolCode(newRawDataBytes, symbolColors, Color.LightBlue);
                this.AddCodeSymbolCodeOptionsItem("Raw Data Bytes", drawableCode, propType);
            }
        }
        private void HandleRawECCBytesChanged(CodeSymbolCode<RawCodeByte> newRawECCBytes)
        {
            var propType = PropertyType.RawECCBytes;

            this.RemoveControlType(propType);

            if (newRawECCBytes != null)
            {
                var symbolColors = new SymbolColors(Color.Orange, Color.Orange, Color.Orange);
                var drawableCode = new DrawableCodeSymbolCode(newRawECCBytes, symbolColors, Color.LightBlue);
                this.AddCodeSymbolCodeOptionsItem("Raw ECC Bytes", drawableCode, propType);
            }
        }
        private void HandleMessageModeChanged(CodeSymbolCode<MessageModeSymbol> newMessageModeSymbol)
        {
            var propType = PropertyType.MessageModeSymbol;

            this.RemoveControlType(propType);

            if(newMessageModeSymbol != null)
            {
                var symbolColors = new SymbolColors(Color.Blue, Color.LightBlue, Color.DarkCyan);
                var drawableCode = new DrawableCodeSymbolCode(newMessageModeSymbol, symbolColors, Color.LightBlue);
                this.AddCodeSymbolCodeOptionsItem("Message Mode", drawableCode, propType);
            }
        }
        private void HandlePaddingBytesChanged(CodeSymbolCode<RawCodeByte> newPaddingBytes)
        {
            var propType = PropertyType.PaddingBytes;

            this.RemoveControlType(propType);

            if (newPaddingBytes != null)
            {
                var symbolColors = new SymbolColors(Color.Blue, Color.LightBlue, Color.DarkCyan);
                var drawableCode = new DrawableCodeSymbolCode(newPaddingBytes, symbolColors, Color.LightBlue);
                this.AddCodeSymbolCodeOptionsItem("Padding Bytes", drawableCode, propType);
            }
        }
        private void HandleEncodedSymbolsChanged(CodeSymbolCode<ByteEncodingSymbol> newEncodedSymbols)
        {
            var propType = PropertyType.EncodedSymbols;

            this.RemoveControlType(propType);

            if (newEncodedSymbols != null)
            {
                var symbolColors = new SymbolColors(Color.Red, Color.LightBlue, Color.Orange);
                var drawableCode = new DrawableCodeSymbolCode(newEncodedSymbols, symbolColors, Color.LightBlue);
                this.AddCodeSymbolCodeOptionsItem("Encoded Symbols", drawableCode, propType);
            }
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
