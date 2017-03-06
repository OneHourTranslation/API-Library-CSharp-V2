using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using oht.Entities;

namespace TestOHT
{
    public partial class Form1 : Form
    {
        #region private methods
        private delegate void CallApi();
        private void ProcessApiCall(CallApi func, bool showOk = true)
        {
            try
            {
                func();
                if(showOk)
                    MessageBox.Show("OK");
            }
            catch (oht.OhtException exc)
            {
                MessageBox.Show(string.Format("Error occured, code: ({0}), message: \"{1}\".", exc.StatusCode, exc.StatusMessage));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Unknown exception: " + exc.Message);
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void butGetAccount_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var acc = api.GetAccountDetails();
                MessageBox.Show("Account User Name: " + acc.AccountUsername);
            };
            ProcessApiCall(c, false);
        }

        private void butResources_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog1.FileName;
                    FileInfo file = new FileInfo(filePath);

                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var response = api.CreateFileResource(string.Empty, fileName: file.Name, fileMime: Mime.GetMimeType(file.Extension), filePath: filePath);
                    textResources.Text = response;                    
                }
            };
            ProcessApiCall(c);
        }

        private void butResourcesContent_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog1.FileName;
                    FileInfo file = new FileInfo(filePath);

                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var response = api.CreateFileResource(Encoding.Default.GetString(File.ReadAllBytes(filePath)), fileMime: Mime.GetMimeType(file.Extension), fileName: file.Name);
                    textResourcesContent.Text = response;                    
                }
            };
            ProcessApiCall(c);
        }

        private void butCreateTextResource_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {                
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                textTextResourceId.Text = api.CreateTextResource(textTextResource.Text);                
            };
            ProcessApiCall(c);
        }

        private void butGetResource_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var resId = textResources.Text;
                if (radioButton2.Checked)
                    resId = textResourcesContent.Text;

                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.GetResource(resId);
                textFileName.Text = response.FileName;
                if (string.IsNullOrEmpty(textFileName.Text))
                    textFileName.Text = response.DownloadUrl;
                textFileMime.Text = response.FileMime;
            };
            ProcessApiCall(c);
        }

        private void butDownloadResource_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var resId = textResources.Text;
                    if (radioButton2.Checked)
                        resId = textResourcesContent.Text;

                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    api.DownloadResource(resId, saveFileDialog1.FileName);                    
                }
            };
            ProcessApiCall(c);
        }

        private void butSupportedLanguages_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.GetSupportedLanguages();
                comboLanguage.DataSource = response;
                comboLanguage.DisplayMember = "LanguageName";
                comboLanguage.ValueMember = "LanguageCode";

                comboSourceLanguage.DisplayMember = "LanguageName";
                comboSourceLanguage.ValueMember = "LanguageCode";
                comboTargetLanguage.DisplayMember = "LanguageName";
                comboTargetLanguage.ValueMember = "LanguageCode";
                comboSourceLanguage.Items.Clear();
                comboTargetLanguage.Items.Clear();
                foreach(var lng in response)
                {
                    comboSourceLanguage.Items.Add(lng);
                    comboTargetLanguage.Items.Add(lng);
                }                
            };
            ProcessApiCall(c);            
        }        

        private void butDetectLnaguage_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.DetectLanguage(textSourceContent.Text);
                textDetectedLanguage.Text = response.Language;
            };
            ProcessApiCall(c);
        }

        private void butSupportedLanguagePairs_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.GetSupportedLanguagePairs();
                comboLanguage.DataSource = null;
                comboLanguage.Items.Clear();
                foreach(var src in response)
                {
                    foreach(var trg in src.Targets)
                    {
                        comboLanguage.Items.Add(src.Source.LanguageName + " | " + trg.LanguageName);
                    }
                }
            };
            ProcessApiCall(c);
        }

        private void butSupportedExpertises_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.GetSupportedExpertises();                
                comboExpertise.Items.Clear();
                comboExpertise.DisplayMember = "Name";
                comboExpertise.ValueMember = "Code";
                foreach (var exp in response)
                {
                    comboExpertise.Items.Add(exp);                    
                }
            };
            ProcessApiCall(c);
        }

        private void butTranslate_Click(object sender, EventArgs e)
        {            
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }
            if (comboTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Target language is required!");
                comboTargetLanguage.Focus();
                return;
            }

            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                textDetectedLanguage.Text = api.MachineTranslation(((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode, ((SupportedLanguage)comboTargetLanguage.SelectedItem).LanguageCode, textSourceContent.Text);
                MessageBox.Show("OK");
            };
            ProcessApiCall(c, false);
        }

        private void butGetWordCount_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.GetWordCount(new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())));
                textWordcount.Text = response.Total.Wordcount.ToString();
            };
            ProcessApiCall(c);
        }
        
        private void butTranslationProject_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }
            if (comboTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Target language is required!");
                comboTargetLanguage.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(textGetQuoteResources.Text))
            {
                MessageBox.Show("At least one source UUID is required!");
                textGetQuoteResources.Focus();
                return;
            }

            CallApi c = delegate
            {
                int wordCount = 0;
                List<string> refResources = null;
                ExpertiseType expertise = ExpertiseType.None;
                string[] custom = null;
                if (!string.IsNullOrWhiteSpace(textRefResources.Text))
                    refResources = new List<string>(textRefResources.Text.Split(",".ToCharArray()));
                if (comboExpertise.SelectedItem != null)
                    expertise = ((SupportedExpertise)comboExpertise.SelectedItem).Code;
                if (!string.IsNullOrWhiteSpace(textCallbackCustom.Text))
                    custom = textCallbackCustom.Text.Split(",".ToCharArray());
                
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.CreateTranslationProject(
                    ((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode, 
                    ((SupportedLanguage)comboTargetLanguage.SelectedItem).LanguageCode,
                    new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())),
                    refResources, wordCount, textNotes.Text, expertise, textProjectName.Text, 
                    textCallbackUrl.Text, custom);

                textProjectUuid.Text = response.ProjectId.ToString();
                
            };
            ProcessApiCall(c);
        }
        
        private void butGetProjectDetails_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                
                int projectId = 0;
                if(Int32.TryParse(textProjectUuid.Text, out projectId))
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var response = api.GetProjectDetails(projectId);
                    MessageBox.Show(string.Format("Project ID: {0}\r\n type: {1}\r\n status: {2}\r\n from language: {3}\r\n to language: {4}", response.ProjectId, response.ProjectType, response.ProjectStatus, response.SourceLanguage, response.TargetLanguage));
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);
        }

        private void butAddProjectComment_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textProjectComment.Text))
            {
                MessageBox.Show("Project comment cannot be empty!");
                textProjectComment.Focus();
                return;
            }

            CallApi c = delegate
            {

                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0 && !string.IsNullOrWhiteSpace(textProjectComment.Text))
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    api.AddProjectComment(projectId, textProjectComment.Text);                    
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c);
        }

        private void butGetQuote_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }
            if (comboTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Target language is required!");
                comboTargetLanguage.Focus();
                return;
            }

            CallApi c = delegate
            {                
                int wordCount = 0;
                if (Int32.TryParse(textWordcount.Text, out wordCount) && wordCount > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var result = api.GetQuote(new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())), wordCount,
                        ((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode,
                        ((SupportedLanguage)comboTargetLanguage.SelectedItem).LanguageCode);

                    MessageBox.Show(string.Format("Quote received. Currency: {0}, word count: {1}, price: {2}.", result.Currency, result.Total.Wordcount, result.Total.Price));
                }
                else
                {
                    MessageBox.Show("Word count is required!");
                    textWordcount.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);
        }

        private void butProofreadingProject_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }            
            if (string.IsNullOrWhiteSpace(textGetQuoteResources.Text))
            {
                MessageBox.Show("At least one source UUID is required!");
                textGetQuoteResources.Focus();
                return;
            }

            CallApi c = delegate
            {
                int wordCount = 0;
                List<string> refResources = null;                
                string[] custom = null;
                if (!string.IsNullOrWhiteSpace(textRefResources.Text))
                    refResources = new List<string>(textRefResources.Text.Split(",".ToCharArray()));               
                if (!string.IsNullOrWhiteSpace(textCallbackCustom.Text))
                    custom = textCallbackCustom.Text.Split(",".ToCharArray());

                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.CreateProofreadingProject(((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode,                    
                    new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())),
                    refResources, wordCount, textNotes.Text, textProjectName.Text, textCallbackUrl.Text, custom);

                textProjectUuid.Text = response.ProjectId.ToString();

            };
            ProcessApiCall(c);
        }

        private void butProofreadingTargetProject_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }
            if (comboTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Target language is required!");
                comboTargetLanguage.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textGetQuoteResources.Text))
            {
                MessageBox.Show("At least one source UUID is required!");
                textGetQuoteResources.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textRefResources.Text))
            {
                MessageBox.Show("At least one translation resource UUID is required!");
                textRefResources.Focus();
                return;
            }


            CallApi c = delegate
            {
                int wordCount = 0;
                List<string> translations = null;
                ExpertiseType expertise = ExpertiseType.None;
                string[] custom = null;
                if (!string.IsNullOrWhiteSpace(textRefResources.Text))
                    translations = new List<string>(textRefResources.Text.Split(",".ToCharArray()));
                if (comboExpertise.SelectedItem != null)
                    expertise = ((SupportedExpertise)comboExpertise.SelectedItem).Code;
                if (!string.IsNullOrWhiteSpace(textCallbackCustom.Text))
                    custom = textCallbackCustom.Text.Split(",".ToCharArray());

                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.CreateProofTranslatedProject(((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode,
                    ((SupportedLanguage)comboTargetLanguage.SelectedItem).LanguageCode,
                    new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())),
                    translations, null, wordCount, textNotes.Text, expertise, textProjectName.Text, textCallbackUrl.Text, custom);

                textProjectUuid.Text = response.ProjectId.ToString();

            };
            ProcessApiCall(c);
        }

        private void butCreateTranscriptionProject_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }            
            if (string.IsNullOrWhiteSpace(textGetQuoteResources.Text))
            {
                MessageBox.Show("At least one source UUID is required!");
                textGetQuoteResources.Focus();
                return;
            }

            CallApi c = delegate
            {
                int wordCount = 0;
                List<string> refResources = null;
                string expertise = "";
                string[] custom = null;
                if (!string.IsNullOrWhiteSpace(textRefResources.Text))
                    refResources = new List<string>(textRefResources.Text.Split(",".ToCharArray()));
                if (comboExpertise.SelectedItem != null)
                    expertise = ((SupportedExpertise)comboExpertise.SelectedItem).Code.GetStringValue();
                if (!string.IsNullOrWhiteSpace(textCallbackCustom.Text))
                    custom = textCallbackCustom.Text.Split(",".ToCharArray());

                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.CreateTranscriptionProject(((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode,                   
                    new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())),
                    refResources, wordCount, textNotes.Text, textProjectName.Text, textCallbackUrl.Text, custom);

                textProjectUuid.Text = response.ProjectId.ToString();

            };
            ProcessApiCall(c);
        }

        private void butCreateWithTranslationProject_Click(object sender, EventArgs e)
        {
            if (comboSourceLanguage.SelectedItem == null)
            {
                MessageBox.Show("Source language is required!");
                comboSourceLanguage.Focus();
                return;
            }
            if (comboTargetLanguage.SelectedItem == null)
            {
                MessageBox.Show("Target language is required!");
                comboTargetLanguage.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textGetQuoteResources.Text))
            {
                MessageBox.Show("At least one source UUID is required!");
                textGetQuoteResources.Focus();
                return;
            }

            CallApi c = delegate
            {
                int wordCount = 0;
                List<string> refResources = null;
                ExpertiseType expertise = ExpertiseType.None;
                string[] custom = null;
                if (!string.IsNullOrWhiteSpace(textRefResources.Text))
                    refResources = new List<string>(textRefResources.Text.Split(",".ToCharArray()));
                if (comboExpertise.SelectedItem != null)
                    expertise = ((SupportedExpertise)comboExpertise.SelectedItem).Code;
                if (!string.IsNullOrWhiteSpace(textCallbackCustom.Text))
                    custom = textCallbackCustom.Text.Split(",".ToCharArray());

                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var response = api.CreateTransProofProject(
                    ((SupportedLanguage)comboSourceLanguage.SelectedItem).LanguageCode,
                    ((SupportedLanguage)comboTargetLanguage.SelectedItem).LanguageCode,
                    new List<string>(textGetQuoteResources.Text.Split(",".ToCharArray())),
                    refResources, wordCount, textNotes.Text, expertise, textProjectName.Text,
                    textCallbackUrl.Text, custom);

                textProjectUuid.Text = response.ProjectId.ToString();

            };
            ProcessApiCall(c);
        }

        private void butGetProjectComments_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {

                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var response = api.GetProjectComments(projectId);
                    string comments = "";
                    foreach(var item in response)
                    {
                        if (!string.IsNullOrWhiteSpace(comments))
                            comments += ", ";
                        comments += item.CommentContent;
                    }

                    textProjectComment.Text = comments;
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c);
        }

        private void butCancelProject_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {

                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    api.CancelProject(projectId);                    
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c);
        }

        private void butRateProject_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {

                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    Dictionary<ServiceRate, bool> serviceRates = new Dictionary<ServiceRate, bool>();
                    List<CustomerRate> customerRates = new List<CustomerRate>();
                    serviceRates.Add(ServiceRate.GoodQuality, false);
                    serviceRates.Add(ServiceRate.SupportHelpful, true);

                    customerRates.Add(CustomerRate.BadWritten);
                    customerRates.Add(CustomerRate.Inconsistent);

                    api.RateProject(projectId, RateType.Customer, 3, serviceRates, customerRates);
                    MessageBox.Show("Project has been rated.");
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);                        
        }

        private void butAddProjectTag_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textProjectTag.Text))
            {
                MessageBox.Show("Tag name is required!");
                textProjectTag.Focus();
                return;
            }

            CallApi c = delegate
            {

                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    api.AddProjectTag(projectId, textProjectTag.Text);
                    MessageBox.Show("Tag added.");
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);
        }

        private void butGetProjectTags_Click(object sender, EventArgs e)
        {            
            CallApi c = delegate
            {
                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    var response = api.GetProjectTags(projectId);
                    var text = "";
                    if(response != null)
                    {
                        foreach (var id in response.Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(text))
                                text += ", ";
                            text += id.ToString();
                        }
                    }                    
                    textProjectTags.Text = text;
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);
        }

        private void butDeleteTag_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textProjectTags.Text))
            {
                MessageBox.Show("At least one tag Id is required!");
                textProjectTags.Focus();
                return;
            }

            CallApi c = delegate
            {
                int projectId = 0;
                if (Int32.TryParse(textProjectUuid.Text, out projectId) && projectId > 0)
                {
                    var ids = textProjectTags.Text.Trim().Split(",".ToCharArray());
                    var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                    foreach(var id in ids)
                    {
                        api.DeleteProjectTag(projectId, System.Convert.ToInt32(id));
                    }
                    MessageBox.Show("Tag(s) deleted.");                 
                }
                else
                {
                    MessageBox.Show("Project ID is required!");
                    textProjectUuid.Focus();
                    return;
                }
            };
            ProcessApiCall(c, false);
        }

        private void butListProjects_Click(object sender, EventArgs e)
        {
            CallApi c = delegate
            {
                
                var api = new oht.OhtApi(txtSecretKey.Text, txtPublicKey.Text, chkUseSandbox.Checked);
                var result = api.GetProjectsList();            
                MessageBox.Show(string.Format("{0} projects found.", result.projectsCount));                
            };
            ProcessApiCall(c, false);
        }
    }
}
