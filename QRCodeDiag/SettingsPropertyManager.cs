using QRCodeDiag.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<StringValueOptionsItem> settingsControls;
        private QRCode qrCode;

        public SettingsPropertyManager(QRCode _qrCode)
        {
            this.settingsControls = new List<StringValueOptionsItem>();
            this.qrCode = _qrCode;

            var versionSizeOptionsItem = new StringValueOptionsItem("Version from Size");
            this.qrCode.VersionChangedEvent += version => { versionSizeOptionsItem.StringValue = version.ToString(); };
            versionSizeOptionsItem.NewValueEnteredEvent += newValue => throw new NotImplementedException();  // ToDo handle writebutton clicked
            this.settingsControls.Add(versionSizeOptionsItem);

            var versionInfo1OptionsItem = new StringValueOptionsItem("Version info 1");


            var versionInfo2OptionsItem = new StringValueOptionsItem("Version info 2");
        }
    }
}
