using QRCodeBaseLib;
using QRCodeBaseLib.DataBlocks;
using QRCodeBaseLib.DataBlocks.SymbolCodes;
using QRCodeBaseLib.ECCDecoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace QRCodeDiagUWP
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
            PreRepairData,
            PreRepairECC,
            PostRepairData,
            PostRepairECC,
            MessageModeSymbol,
            CharCountIndicator,
            PaddingBytes,
            EncodedSymbols,
            TerminatorSymbol
        }

        private QRCode qrCode;
        private readonly DrawingManager drawingManager;
        private readonly Dictionary<PropertyType, List<CodeOptions>> settingsControls;
        private readonly Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> drawingProperties;
        private readonly StackPanel stackPanel;

        public SettingsPropertyManager(DrawingManager setDrawingManager, StackPanel setStackPanel)
        {
            this.drawingManager = setDrawingManager;
            this.settingsControls = new Dictionary<PropertyType, List<CodeOptions>>();
            this.drawingProperties = CreateDrawingProperties();
            this.stackPanel = setStackPanel;
        }

        private static Dictionary<PropertyType, CodeSymbolCodeDrawingProperties> CreateDrawingProperties()
        {
            return new Dictionary<PropertyType, CodeSymbolCodeDrawingProperties>
            {
                [PropertyType.RawCode] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Orange, Colors.Orange, Colors.Orange), Colors.Orange, "Raw Code"),
                [PropertyType.RawDataBytes] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.DarkOrange, Colors.DarkOrange, Colors.DarkOrange), Colors.LightBlue, "Raw Data Bytes"),
                [PropertyType.RawECCBytes] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Orange, Colors.Orange, Colors.Orange), Colors.LightBlue, "Raw ECC Bytes"),
                [PropertyType.PreRepairData] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Orange, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Pre repair data block"),
                [PropertyType.PreRepairECC] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Orange, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Pre repair ecc block"),
                [PropertyType.PostRepairData] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.DarkOrange, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Post repair data block"),
                [PropertyType.PostRepairECC] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.DarkOrange, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Post repair ecc block"),
                [PropertyType.MessageModeSymbol] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Blue, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Message Mode Symbol"),
                [PropertyType.CharCountIndicator] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Blue, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Char Count Indicator"),
                [PropertyType.PaddingBytes] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Blue, Colors.LightBlue, Colors.DarkCyan), Colors.LightBlue, "Padding Bytes"),
                [PropertyType.EncodedSymbols] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Red, Colors.LightBlue, Colors.Orange), Colors.LightBlue, "Encoded Symbols"),
                [PropertyType.TerminatorSymbol] = new CodeSymbolCodeDrawingProperties(new SymbolColors(Colors.Red, Colors.LightBlue, Colors.Orange), Colors.LightBlue, "Terminator Symbol"),
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
            qrCode.InterleavingBlocksChangedEvent -= HandleInterleavingBlocksChanged;
            qrCode.EncodedMessagesChangedEvent -= HandleEncodedMessagesChanged;
        }
        private void RegisterEventHandlers()
        {
            qrCode.RawCodeChangedEvent += HandleRawCodeChanged;
            qrCode.RawDataBytesChangedEvent += HandleRawDataBytesChanged;
            qrCode.RawECCBytesChangedEvent += HandleRawECCBytesChanged;
            qrCode.PaddingBytesChangedEvent += HandlePaddingBytesChanged;
            qrCode.TerminatorSymbolCodeChangedEvent += HandleTerminatorSymbolChanged;
            qrCode.InterleavingBlocksChangedEvent += HandleInterleavingBlocksChanged;
            qrCode.EncodedMessagesChangedEvent += HandleEncodedMessagesChanged;
        }

        private void RemoveControlTypeElement(PropertyType ctrlType, int index)
        {
            if (this.settingsControls.TryGetValue(ctrlType, out var optionsItemList))
            {
                if (optionsItemList.Count >= index)
                {
                    var optionsItem = optionsItemList[index];
                    optionsItemList.RemoveAt(index);

                    optionsItem.PropertyChangedEvent -= this.PropertyChangedEvent;
                    this.drawingManager.UnregisterCodeSymbolCode(optionsItem.DrawableCodeSymbolCode);

                    if (this.stackPanel.Children.Contains(optionsItem))
                    {
                        this.stackPanel.Children.Remove(optionsItem);
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

        private void AddCodeOptions(DrawableCodeSymbolCode drawableCode, PropertyType ctrlType)
        {
            var newOptionsItem = new CodeOptions(drawableCode.DisplayName, drawableCode);

            newOptionsItem.PropertyChangedEvent += this.PropertyChangedEvent;
            this.stackPanel.Children.Add(newOptionsItem);
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
            List<CodeOptions> optionsItemList;

            // create list, if not existing yet
            if (!this.settingsControls.TryGetValue(propertyType, out optionsItemList))
            {
                optionsItemList = new List<CodeOptions>();
                this.settingsControls[propertyType] = optionsItemList;
            }

            var newCount = codeSymbolCodes.Count();

            // remove excessive trailing old items
            for (int i = optionsItemList.Count; i > newCount; i--)
                RemoveControlTypeElement(propertyType, i - 1);

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

                this.AddCodeOptions(drawableCode, propertyType);
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
        private void HandleInterleavingBlocksChanged(IEnumerable<ECCBlock> newInterleavingBlocks)
        {
            var preRepData = from blk in newInterleavingBlocks select blk.GetPreRepairData();
            var preRepECC = from blk in newInterleavingBlocks select blk.GetPreRepairECC();
            var postRepData = from blk in newInterleavingBlocks where blk.RepairSuccess select blk.GetPostRepairData();
            var postRepECC = from blk in newInterleavingBlocks where blk.RepairSuccess select blk.GetPostRepairECC();

            this.HandleCodeChange(preRepData, PropertyType.PreRepairData);
            this.HandleCodeChange(preRepECC, PropertyType.PreRepairECC);
            this.HandleCodeChange(postRepData, PropertyType.PreRepairData);
            this.HandleCodeChange(postRepECC, PropertyType.PreRepairECC);
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

            if (this.qrCode != null)
                this.RegisterEventHandlers();
        }
        #endregion
    }
}
