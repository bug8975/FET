using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BenNHControl
{
    public partial class Toast : Form
    {
        #region Variables

        /// <summary>
        /// The list of currently open Toasts.
        /// </summary>
        private static List<Toast> openForms = new List<Toast>();

        /// <summary>
        /// Indicates whether the form can receive focus or not.
        /// </summary>
        private bool allowFocus = false;
        /// <summary>
        /// The object that creates the sliding animation.
        /// </summary>
        private FormAnimator animator;
        /// <summary>
        /// The handle of the window that currently has focus.
        /// </summary>
        private IntPtr currentForegroundWindow;

        #endregion // Variables

        #region APIs

        /// <summary>
        /// Gets the handle of the window that currently has focus.
        /// </summary>
        /// <returns>
        /// The handle of the window that currently has focus.
        /// </returns>
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Activates the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// The handle of the window to be focused.
        /// </param>
        /// <returns>
        /// True if the window was focused; False otherwise.
        /// </returns>
        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion // APIs

        #region Constructors

        /// <summary>
        /// Creates a new Toast object that is displayed for the specified length of time.
        /// </summary>
        /// <param name="lifeTime">
        /// The length of time, in milliseconds, that the form will be displayed.
        /// </param>
        public Toast(int lifeTime, string message)
        {
            InitializeComponent();

            // Set the time for which the form should be displayed and the message to display.
            this.lifeTimer.Interval = lifeTime;
            this.messageLabel.Text = message;

            // Display the form by sliding up.
            this.animator = new FormAnimator(this,
                                             FormAnimator.AnimationMethod.Slide,
                                             FormAnimator.AnimationDirection.Up,
                                             500);
        }
        public Toast()
        {
            InitializeComponent();
            // Display the form by sliding up.
            this.animator = new FormAnimator(this,
                                             FormAnimator.AnimationMethod.Slide,
                                             FormAnimator.AnimationDirection.Up,
                                             500);
        }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// Displays the form.
        /// </summary>
        /// <remarks>
        /// Required to allow the form to determine the current foreground window     before being displayed.
        /// </remarks>
        public new void Show()
        {
            // Determine the current foreground window so it can be reactivated each time this form tries to get the focus.
            this.currentForegroundWindow = GetForegroundWindow();

            // Display the form.
            base.Show();
        }

        public void ShowInfo(int lifeTime, string message)
        {
            this.lifeTimer.Interval = lifeTime;
            this.messageLabel.Text = message;
            Show();
        }

        #endregion // Methods

        #region Event Handlers

        private void Toast_Load(object sender, EventArgs e)
        {
            // Display the form just above the system tray.
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 5,
                                      Screen.PrimaryScreen.WorkingArea.Height - this.Height - 5);

            // Move each open form upwards to make room for this one.
            foreach (Toast openForm in Toast.openForms)
            {
                openForm.Top -= this.Height + 5;
            }

            // Add this form from the open form list.
            Toast.openForms.Add(this);

            // Start counting down the form's liftime.
            this.lifeTimer.Start();
        }

        private void Toast_Activated(object sender, EventArgs e)
        {
            // Prevent the form taking focus when it is initially shown.
            if (!this.allowFocus)
            {
                // Activate the window that previously had the focus.
                SetForegroundWindow(this.currentForegroundWindow);
            }
        }

        private void Toast_Shown(object sender, EventArgs e)
        {
            // Once the animation has completed the form can receive focus.
            this.allowFocus = true;

            // Close the form by sliding down.
            this.animator.Direction = FormAnimator.AnimationDirection.Down;
        }

        private void Toast_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Move down any open forms above this one.
            foreach (Toast openForm in Toast.openForms)
            {
                if (openForm == this)
                {
                    // The remaining forms are below this one.
                    break;
                }

                openForm.Top += this.Height + 5;
            }

            // Remove this form from the open form list.
            Toast.openForms.Remove(this);
        }

        private void lifeTimer_Tick(object sender, EventArgs e)
        {
            // The form's lifetime has expired.
            this.Close();
        }

        #endregion // Event Handlers

    }
}
