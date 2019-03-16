using QRCodeDiag.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag
{
    class SettingsPropertyManager
    {
        enum SettingsProperties
        {
            VersionNumber,
            ECCLevel,
            EncodingType,
            MessageContent,
            MessageLength,
            Padding,
            Terminator,
            MaskType,

        }

        private List<UserControl> settingsControls;

        public SettingsPropertyManager(QRCode qrCode)
        {
            this.settingsControls = new List<UserControl>();

            var versionSizeOptionsItem = new StringValueOptionsItem("Version from Size");
            qrCode.VersionChangedEvent += version => { versionSizeOptionsItem.StringValue = version.ToString(); };
            versionSizeOptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();  // ToDo handle writebutton clicked
            this.settingsControls.Add(versionSizeOptionsItem);

            //ToDo implement version info in QRCode first
            //var versionInfo1OptionsItem = new StringValueOptionsItem("Version info 1");
            //qrCode.VersionInfo1ChangedEvent += versionInfo1 => { versionInfo1OptionsItem.StringValue = versionInfo1.ToString(); };
            //versionInfo1OptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();  // ToDo handle writebutton clicked
            //this.settingsControls.Add(versionInfo1OptionsItem);

            //var versionInfo2OptionsItem = new StringValueOptionsItem("Version info 2");
            //qrCode.VersionInfo2ChangedEvent += versionInfo2 => { versionInfo2OptionsItem.StringValue = versionInfo2.ToString(); };
            //versionInfo1OptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();  // ToDo handle writebutton clicked
            //this.settingsControls.Add(versionInfo2OptionsItem);

            var messageOptionsItem = new StringValueOptionsItem("Message");
            qrCode.MessageChangedEvent += (msg, valid) => messageOptionsItem.StringValue = msg; // ToDo inform the user if the message is valid or an error message is displayed
            //messageOptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();   // ToDo write only message value in message field or adapt length indicator (maybe different length indicator allowed to write only partial messages)? (re)create ECC for message?
            this.settingsControls.Add(messageOptionsItem);

            var highlightPaddingOptionsItem = new BooleanValueOptionsItem("Padding bytes", "Highlight padding");
            //ToDo what to do if clicked
            this.settingsControls.Add(highlightPaddingOptionsItem);

            var highlightMessageOptionsItem = new BooleanValueOptionsItem("Message Symbols", "Highlight message symbols");
            this.settingsControls.Add(highlightMessageOptionsItem);

            var highlightRawBytesOptionsItem = new BooleanValueOptionsItem("Raw bytes", "Highlight raw bytes");
            this.settingsControls.Add(highlightRawBytesOptionsItem);

            //ToDo show/hide all ECC blocks/interleaved data blocks individually
        }
    }
}
