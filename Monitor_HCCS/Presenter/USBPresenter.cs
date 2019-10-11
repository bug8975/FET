using Monitor_HCCS.View;

namespace Monitor_HCCS.Presenter
{
    public class USBPresenter
    {
        public IUSBView UsbView { get; set; }

        public USBPresenter(IUSBView usbView)
        {
            UsbView = usbView;

        }

    }
}
