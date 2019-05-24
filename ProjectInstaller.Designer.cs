namespace SampleWindowsService {

    using System.Diagnostics;
    using System.Security.Principal;
    using System.ServiceProcess;

    partial class ProjectInstaller {

        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>ServiceInstaller instance to register and manage service</summary>
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        /// <summary>ServiceProcessInstaller instance to manage installation of service</summary>
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            // Construct components as a Container instance
            this.components = new System.ComponentModel.Container();

            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();

            // serviceInstaller
            this.serviceInstaller.Description = "ConstantsSystemServiceDesc";
            this.serviceInstaller.DisplayName = "ConstantsSystemServiceName";
            this.serviceInstaller.ServiceName = "ConstantsSystemNtServiceName";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

            // serviceProcessInstaller
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;

            // ServiceInstaller
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                this.serviceProcessInstaller,
                this.serviceInstaller
            });

            // Add handler to automatically start service on install
            serviceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(serviceInstaller_AfterInstall);
        }

        /// <summary>
        /// After install, start the service
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">InstallEventArgs object</param>
        private void serviceInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e) {

            // Get current privilege
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            // Create event source if it does not exist, and we have administrator privileges
            if (principal.IsInRole(WindowsBuiltInRole.Administrator)) {
                if (!EventLog.SourceExists("ConstantsSystemNtServiceName")) {
                    EventLog.CreateEventSource("ConstantsSystemNtServiceName", "Application");
                }
            }

            var sc = new ServiceController("ConstantsSystemNtServiceName");
            sc.Start();
        }
        #endregion
    }
}
