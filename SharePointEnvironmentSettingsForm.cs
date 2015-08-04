#region Copyright Notice
// 
// (C) 2009 Quest Software, Inc.
// ALL RIGHTS RESERVED.
//
// This software is the confidential and proprietary information of
// Quest Software Inc. ("Confidential Information"). You shall not
// disclose such Confidential Information and shall use it only in
// accordance with the terms of the license agreement you entered
// into with Quest Software Inc.
//
// QUEST SOFTWARE INC. MAKES NO REPRESENTATIONS OR
// WARRANTIES ABOUT THE SUITABLIITY OF THE SOFTWARE,
// EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE, OR NON-INFRINGMENT. QUEST
// SOFTWARE SHALL NOT BE LIABLE FOR ANY DAMAGES
// SUFFERED BY LICENSEE AS A RESULT OF USING, MODIFYING
// OR DISTRIBUTING THIS SOFTWARE OR ITS DERIVATIVES.
//
#endregion

using System;
using System.Windows.Forms;
using Quest.NSP.SharePoint;

namespace Quest.NSP.Migrator
{
	public partial class SharePointEnvironmentSettingsForm : Form
	{
		private SharePointEnvironmentSettings environmentSettings;

		public enum HeadingType
		{
			None,
			NewEnvironment,
			ReviewSettings
		}

		public SharePointEnvironmentSettingsForm(SharePointEnvironmentSettings environmentSettings, HeadingType headingType)
		{
			InitializeComponent();

			if (headingType != HeadingType.None)
			{
				lblReviewHeading.Visible = true;

				if (headingType == HeadingType.NewEnvironment)
					lblReviewHeading.Text = "Please review the settings for this new environment";
			}

			SharePointEnvironmentSettings = environmentSettings.GetCopy();
		}

		public SharePointEnvironmentSettings SharePointEnvironmentSettings
		{
			get
			{
				if (environmentSettings is ImportServiceEnvironmentSettings)
				{
					ImportServiceEnvironmentSettings settings = (ImportServiceEnvironmentSettings)environmentSettings;
					settings.ClientSideLinkTrackingEnabled = chkLinkTrackingEnabled.Checked;
					settings.UseDefaultLinkTrackingRedirectorUrl = rdbDefaultRedirectorUrl.Checked;
					settings.CustomLinkTrackingRedirectorUrl = txtRedirectorUrl.Text;
					settings.ClientSideUserMappingEnabled = chkClientSideUserMapping.Checked;
					settings.SharedFolderForDataTransferEnabled = chkUseSharedFolder.Checked;
				}
				else if (environmentSettings is ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings)
				{
					ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings settings = (ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings)environmentSettings;
					settings.LinkTrackingEnabled = chkLinkTrackingEnabled.Checked;
					settings.UseDefaultLinkTrackingRedirectorUrl = rdbDefaultRedirectorUrl.Checked;
					settings.CustomLinkTrackingRedirectorUrl = txtRedirectorUrl.Text;
					settings.CredentialsExpirationWindow = uint.Parse(txtCredentialsExpirationWindow.Text);
					settings.CredentialsExpirationInterval = chkOverrideCookieExpiration.Checked ? uint.Parse(txtCookieExpirationOverride.Text) : 0;
					settings.SaveCredentials = chkSaveCredentials.Checked;
                    settings.BackoffInterval = uint.Parse(txtBackoffInterval.Text);
                    settings.RetryCount = uint.Parse(txtRetryCount.Text);
				}
				else if (environmentSettings is ClientWebServicesEnvironmentSettings)
				{
					ClientWebServicesEnvironmentSettings settings = (ClientWebServicesEnvironmentSettings)environmentSettings;
					settings.LinkTrackingEnabled = chkLinkTrackingEnabled.Checked;
					settings.UseDefaultLinkTrackingRedirectorUrl = rdbDefaultRedirectorUrl.Checked;
					settings.CustomLinkTrackingRedirectorUrl = txtRedirectorUrl.Text;
				}
				else if (environmentSettings is LocalSharePointEnvironmentSettings)
				{
					LocalSharePointEnvironmentSettings settings = (LocalSharePointEnvironmentSettings)environmentSettings;
					settings.LinkTrackingEnabled = chkLinkTrackingEnabled.Checked;
					settings.UseDefaultLinkTrackingRedirectorUrl = rdbDefaultRedirectorUrl.Checked;
					settings.CustomLinkTrackingRedirectorUrl = txtRedirectorUrl.Text;
				}

				return environmentSettings;
			}

			set
			{
				this.environmentSettings = value;

				Panel activePanel = null;

				if (value is ImportServiceEnvironmentSettings)
				{
					activePanel = pnlImportService;

					ImportServiceEnvironmentSettings settings = (ImportServiceEnvironmentSettings)value;
					chkLinkTrackingEnabled.Checked = settings.ClientSideLinkTrackingEnabled;
					if (settings.UseDefaultLinkTrackingRedirectorUrl)
						rdbDefaultRedirectorUrl.Checked = true;
					else
						rdbUserDefinedRedirectorUrl.Checked = true;
					txtRedirectorUrl.Text = settings.CustomLinkTrackingRedirectorUrl;
					chkClientSideUserMapping.Checked = settings.ClientSideUserMappingEnabled;
					chkUseSharedFolder.Checked = settings.SharedFolderForDataTransferEnabled;

					chkLinkTrackingEnabled.Text = "Perform direct writes to the Link Tracking database from clients (overrides server settings)";
				}
				else if (environmentSettings is ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings)
				{
					activePanel = pnlWebServicesClaimsBasedAuth;

					ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings settings = (ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings)value;
					chkLinkTrackingEnabled.Checked = settings.LinkTrackingEnabled;
					if (settings.UseDefaultLinkTrackingRedirectorUrl)
						rdbDefaultRedirectorUrl.Checked = true;
					else
						rdbUserDefinedRedirectorUrl.Checked = true;
					txtRedirectorUrl.Text = settings.CustomLinkTrackingRedirectorUrl;
					txtCredentialsExpirationWindow.Text = settings.CredentialsExpirationWindow.ToString();
					chkOverrideCookieExpiration.Checked = txtCookieExpirationOverride.Enabled = (settings.CredentialsExpirationInterval > 0);
					if (settings.CredentialsExpirationInterval > 0)
						txtCookieExpirationOverride.Text = settings.CredentialsExpirationInterval.ToString();
					chkSaveCredentials.Checked = settings.SaveCredentials;

                    txtBackoffInterval.Text = settings.BackoffInterval.ToString();
                    txtRetryCount.Text = settings.RetryCount.ToString();
                    if (environmentSettings is ClientWebServicesOffice365AuthenticationEnvironmentSettings)
                    {
                        lblWebServicesClaimsBasedAuth.Text = "Web Services Environment Settings (Office 365 Authentication)";
                    }
				}
				else if (environmentSettings is ClientWebServicesEnvironmentSettings)
				{
					activePanel = pnlWebServicesClassicModeAuth;

					ClientWebServicesEnvironmentSettings settings = (ClientWebServicesEnvironmentSettings)value;
					chkLinkTrackingEnabled.Checked = settings.LinkTrackingEnabled;
					if (settings.UseDefaultLinkTrackingRedirectorUrl)
						rdbDefaultRedirectorUrl.Checked = true;
					else
						rdbUserDefinedRedirectorUrl.Checked = true;
					txtRedirectorUrl.Text = settings.CustomLinkTrackingRedirectorUrl;
				}
				else if (environmentSettings is LocalSharePointEnvironmentSettings)
				{
					activePanel = pnlSharePointServer;

					LocalSharePointEnvironmentSettings settings = (LocalSharePointEnvironmentSettings)value;
					chkLinkTrackingEnabled.Checked = settings.LinkTrackingEnabled;
					if (settings.UseDefaultLinkTrackingRedirectorUrl)
						rdbDefaultRedirectorUrl.Checked = true;
					else
						rdbUserDefinedRedirectorUrl.Checked = true;
					txtRedirectorUrl.Text = settings.CustomLinkTrackingRedirectorUrl;
				}

				activePanel.Visible = true;
				pnlLinkTracking.Location = new System.Drawing.Point(activePanel.Location.X, activePanel.Location.Y + activePanel.Height);
			}
		}

		private void chkLinkTrackingEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			rdbDefaultRedirectorUrl.Enabled = chkLinkTrackingEnabled.Checked;
			rdbUserDefinedRedirectorUrl.Enabled = chkLinkTrackingEnabled.Checked;
			lblRedirectorUrl.Enabled = chkLinkTrackingEnabled.Checked;
			txtRedirectorUrl.Enabled = chkLinkTrackingEnabled.Checked;
			lblRedirectorExample.Enabled = chkLinkTrackingEnabled.Checked;
		}
	
		private void rdbRedirectorUrl_CheckedChanged(object sender, System.EventArgs e)
		{
			SetLinkTrackingRedirectorUrl(rdbUserDefinedRedirectorUrl.Checked);
		}

		private void chkOverrideCookieExpiration_CheckedChanged(object sender, EventArgs e)
		{
			txtCookieExpirationOverride.Enabled = chkOverrideCookieExpiration.Checked;
		}

		private void SharePointEnvironmentSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK && environmentSettings is ClientWebServicesClaimsBasedAuthenticationEnvironmentSettings)
			{
				bool expirationWindowValid = true;

				uint expirationWindow = 0;
				if (!uint.TryParse(txtCredentialsExpirationWindow.Text, out expirationWindow) || expirationWindow == 0 || expirationWindow > 1440)
				{
                    MessageBox.Show("The cookie refresh interval must be a value between 1 and 1440 minutes.", "Invalid number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
					e.Cancel = true;
					expirationWindowValid = false;
				}

				if (chkOverrideCookieExpiration.Checked)
				{
					uint expirationOverride = 0;
					if (!uint.TryParse(txtCookieExpirationOverride.Text, out expirationOverride) || expirationOverride == 0 || expirationOverride > 1440)
					{
                        MessageBox.Show("The cookie expiration interval must be a value between 1 and 1440 minutes.", "Invalid number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
						e.Cancel = true;
					}
					else if (expirationWindowValid && expirationWindow >= expirationOverride)
					{
                        MessageBox.Show("The cookie expiration interval must be greater than the cookie refresh interval.", "Invalid number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
						e.Cancel = true;
					}
				}

                uint backoffInterval = 0;
                if (!uint.TryParse(txtBackoffInterval.Text, out backoffInterval) || backoffInterval < 30)
                {
                    MessageBox.Show("Back off uses interval must be greater than 30 seconds", "Invalid number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
                uint retryCount = 0;
                if (!uint.TryParse(txtRetryCount.Text, out retryCount) || retryCount < 5)
                {
                    MessageBox.Show("Retries must be greater than 5 times", "Invalid number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
			}
		}

		private void SetLinkTrackingRedirectorUrl(bool enabled)
		{
			lblRedirectorUrl.Visible = enabled;
			txtRedirectorUrl.Visible = enabled;
			lblRedirectorExample.Visible = enabled;
		}
	}
}
