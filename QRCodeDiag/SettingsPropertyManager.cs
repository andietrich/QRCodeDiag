using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks;
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
            RawCode,
            RawDataBytes,
            RawECCBytes,
            MessageModeSymbol,
            CharCountIndicator,
            PaddingBytes,
            EncodedSymbols,
            TerminatorSymbol
        }

        private QRCode qrCode;
        private readonly DrawingManager drawingManager;
        private readonly Dictionary<PropertyType, List<CodeSymbolCodeOptionsItem>> settingsControls;
        private readonly Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> drawingProperties;
        private readonly ControlCollection ctrlCollection;

        public SettingsPropertyManager(DrawingManager setDrawingManager, ControlCollection setCtrlCollection)
        {
            this.drawingManager = setDrawingManager;
            this.settingsControls = new Dictionary<PropertyType, List<CodeSymbolCodeOptionsItem>>();
            this.drawingProperties = CreateDrawingProperties();
            this.ctrlCollection = setCtrlCollection;
        }

        private static Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> CreateDrawingProperties()
        {
            return new Dictionary<PropertyType, CodeSymbolCodeDrawingProperties>
            {
                [PropertyType.RawCode]            = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Orange,     Color.Orange,     Color.Orange    ), Color.Orange,    "Raw Code"            ),
                [PropertyType.RawDataBytes]       = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.DarkOrange, Color.DarkOrange, Color.DarkOrange), Color.LightBlue, "Raw Data Bytes"      ),
                [PropertyType.RawECCBytes]        = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Orange,     Color.Orange,     Color.Orange    ), Color.LightBlue, "Raw ECC Bytes"       ),
                [PropertyType.MessageModeSymbol]  = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Blue,       Color.LightBlue,  Color.DarkCyan  ), Color.LightBlue, "Message Mode Symbol" ),
                [PropertyType.CharCountIndicator] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Blue,       Color.LightBlue,  Color.DarkCyan  ), Color.LightBlue, "Char Count Indicator"),
                [PropertyType.PaddingBytes]       = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Blue,       Color.LightBlue,  Color.DarkCyan  ), Color.LightBlue, "Padding Bytes"       ),
                [PropertyType.EncodedSymbols]     = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Red,        Color.LightBlue,  Color.Orange    ), Color.LightBlue, "Encoded Symbols"     ),
                [PropertyType.TerminatorSymbol]   = new CodeSymbolCodeDrawingProperties(new SymbolColors(Color.Red,        Color.LightBlue,  Color.Orange    ), Color.LightBlue, "Terminator Symbol"   ),
            };
        }

        #region private methods
        private void UnregisterEventHandlers()
        {
            qrCode.RawCodeChangedEvent -= HandleRawCodeChanged;
            qrCode.RawDataBytesChangedEvent -= HandleRawDataBytesChanged;
            qrCode.RawECCBytesChangedEvent -= HandleRawECCBytesChanged;
            qrCode.PaddingBytesChangedEvent -= HandlePaddingBytesChanged;
            qrCode.TerminatorSymbolCodeChangedEvent -= HandleTerminatorSymbolChanged;
            qrCode.EncodedMessagesChangedEvent -= HandleEncodedMessagesChanged;
        }
        private void RegisterEventHandlers()
        {
            qrCode.RawCodeChangedEvent += HandleRawCodeChanged;
            qrCode.RawDataBytesChangedEvent += HandleRawDataBytesChanged;
            qrCode.RawECCBytesChangedEvent += HandleRawECCBytesChanged;
            qrCode.PaddingBytesChangedEvent += HandlePaddingBytesChanged;
            qrCode.TerminatorSymbolCodeChangedEvent += HandleTerminatorSymbolChanged;
            qrCode.EncodedMessagesChangedEvent += HandleEncodedMessagesChanged;
        }
        //private void RegisterMessageContentListener()
        //{
        //    var messageOptionsItem = new StringValueOptionsItem("Message");

        //    messageOptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();   // ToDo write only message value in message field or adapt length indicator (maybe different length indicator allowed to write only partial messages)? (re)create ECC for message?
        //    this.settingsControls[SettingsProperties.MessageContent]= messageOptionsItem;
        //    this.qrCode.MessageChangedEvent += (msg, valid) => messageOptionsItem.StringValue = msg; // ToDo inform the user if the message is valid or an error message is displayed
        //}

        /// <summary>
        /// If a Control with index <paramref name="index"/>exists for the given <paramref name="ctrlType"/> this function removes it
        /// from <cref>ctrlCollection</cref> and <cref>settingsControls</cref>.
        /// The associated <cref>DrawableCodeSymbolCode</cref> is also removed from <cref>drawingManager</cref>
        /// </summary>
        /// <param name="ctrlType">Type of control to be removed</param>
        /// <param name="index">Index of the control to be removed</param>
        private void RemoveControlTypeElement(PropertyType ctrlType, int index)
        {
            if (this.settingsControls.TryGetValue(ctrlType, out var optionsItemList))
            {
                if(optionsItemList.Count >= index)
                {
                    var optionsItem = optionsItemList[index];
                    optionsItemList.RemoveAt(index);

                    optionsItem.PropertyChangedEvent -= this.PropertyChangedEvent;
                    this.drawingManager.UnregisterCodeSymbolCode(optionsItem.DrawableCodeSymbolCode);

                    if (this.ctrlCollection.Contains(optionsItem))
                    {
                        this.ctrlCollection.Remove(optionsItem);
                    }
                }
            }
        }

        private void RemoveAllControlTypes()
        {
            foreach (PropertyType pType in Enum.GetValues(typeof(PropertyType)))
            {
                if (this.settingsControls.TryGetValue(pType, out var optionsItemList))
                {
                    for (int i = 0; i < optionsItemList.Count; i++)
                    {
                        RemoveControlTypeElement(pType, i);
                    }
                }
            }
        }

        private void AddCodeSymbolCodeOptionsItem(DrawableCodeSymbolCode drawableCode, PropertyType ctrlType)
        {
            var newOptionsItem = new CodeSymbolCodeOptionsItem(drawableCode.DisplayName, drawableCode);

            newOptionsItem.PropertyChangedEvent += this.PropertyChangedEvent;
            this.ctrlCollection.Add(newOptionsItem);
            this.drawingManager.RegisterCodeSymbolCode(drawableCode);
            this.settingsControls[ctrlType].Add(newOptionsItem);
        }

        private void HandleUniqueCodeChange(ICodeSymbolCode newCodeSymbolCode, PropertyType propertyType)
        {
            var symbolList = new List<ICodeSymbolCode>();

            if (newCodeSymbolCode != null)
                symbolList.Add(newCodeSymbolCode);

            this.HandleCodeChange(symbolList, propertyType);
        }

        private void HandleCodeChange(IEnumerable<ICodeSymbolCode> codeSymbolCodes, PropertyType propertyType)
        {
            List<CodeSymbolCodeOptionsItem> optionsItemList;

            // create list, if not existing yet
            if (!this.settingsControls.TryGetValue(propertyType, out optionsItemList))
            {
                optionsItemList = new List<CodeSymbolCodeOptionsItem>();
                this.settingsControls[propertyType] = optionsItemList;
            }

            var newCount = codeSymbolCodes.Count();

            // remove excessive trailing old items
            for (int i = newCount; i < optionsItemList.Count; i++)
                RemoveControlTypeElement(propertyType, i);

            // update existing item
            for (int i = 0; i < optionsItemList.Count; i++)
            {
                var newDrawableCode = new DrawableCodeSymbolCode(codeSymbolCodes.ElementAt(i), this.drawingProperties[propertyType]);
                var optionsItem = optionsItemList[i];

                this.drawingManager.UnregisterCodeSymbolCode(optionsItem.DrawableCodeSymbolCode);
                optionsItem.DrawableCodeSymbolCode = newDrawableCode;
                this.drawingManager.RegisterCodeSymbolCode(newDrawableCode);
            }

            // add new items
            for (int i = optionsItemList.Count; i < newCount; i++)
            {
                var drawableCode = new DrawableCodeSymbolCode(codeSymbolCodes.ElementAt(i), this.drawingProperties[propertyType]);

                this.AddCodeSymbolCodeOptionsItem(drawableCode, propertyType);
            }
        }

        private void HandleRawCodeChanged(ICodeSymbolCode newRawCode)
        {
            this.HandleUniqueCodeChange(newRawCode, PropertyType.RawCode);
        }
        private void HandleRawDataBytesChanged(ICodeSymbolCode newRawDataBytes)
        {
            this.HandleUniqueCodeChange(newRawDataBytes, PropertyType.RawDataBytes);
        }
        private void HandleRawECCBytesChanged(ICodeSymbolCode newRawECCBytes)
        {
            this.HandleUniqueCodeChange(newRawECCBytes, PropertyType.RawECCBytes);
        }
        private void HandlePaddingBytesChanged(ICodeSymbolCode newPaddingBytes)
        {
            this.HandleUniqueCodeChange(newPaddingBytes, PropertyType.PaddingBytes);
        }
        private void HandleTerminatorSymbolChanged(ICodeSymbolCode newTerminatorSymbol)
        {
            this.HandleUniqueCodeChange(newTerminatorSymbol, PropertyType.TerminatorSymbol);
        }
        private void HandleEncodedMessagesChanged(IEnumerable<EncodedMessage> newEncodedMessages)
        {
            var messageModeCodes = from msg in newEncodedMessages where msg.MessageModeSymbolCode != null select msg.MessageModeSymbolCode;
            var charCountCodes = from msg in newEncodedMessages where msg.CharCountIndicatorSymbolCode != null select msg.CharCountIndicatorSymbolCode;
            var messageCodes = from msg in newEncodedMessages where msg.Message != null select msg.Message;

            this.HandleCodeChange(messageModeCodes, PropertyType.MessageModeSymbol);
            this.HandleCodeChange(charCountCodes, PropertyType.CharCountIndicator);
            this.HandleCodeChange(messageCodes, PropertyType.EncodedSymbols);
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
