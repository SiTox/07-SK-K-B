using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

#region newusing
using Bankomats;
using System.Workflow.Runtime;
using Interfaces;
using System.Collections;
using System.Workflow.Runtime.Hosting;
using System.Workflow.ComponentModel;
using System.Workflow.Activities.Rules;
using System.CodeDom;
using System.Workflow.Runtime.Tracking;
using System.Workflow.Activities;
#endregion

namespace Bankomats
{
    public partial class MainForm : Form, IFormService
    {


        #region Status Variables
        private CultureInfo _currentAccountCulture;
        public CultureInfo currentAccountCulture
        {
            get { return _currentAccountCulture; }
            set
            {
                _currentAccountCulture = value;
                status.Text = _currentAccountCulture.DisplayName;
            }
        }
        #endregion

        #region Ui var
        public Button[,] BMenu;
        public Button[] BAction;
        #endregion

        #region WorkFlow Variables
        private WorkflowRuntime workflowRuntime;
        private WorkflowInstance workflowInstance;
        #endregion

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

        public MainForm()
        {
            InitializeComponent();

            #region UI define
            GenerateButtons();
            GenerateAcctions();
            #endregion


            #region Bankomats Init
    
            currentAccountCulture = CultureInfo.CurrentCulture;

            #endregion

            #region IniT Workflow
            workflowRuntime = new WorkflowRuntime();
            ExternalDataExchangeService des = new ExternalDataExchangeService();
            workflowRuntime.AddService(des);
            des.AddService(this);

            workflowRuntime.StartRuntime();

            workflowRuntime.WorkflowCompleted += OnWorkflowCompleted;
            workflowRuntime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(workflowRuntime_WorkflowTerminated);

            Type type = typeof(BankomatWorkflowLibrary.BankomatsWorkflow);
            workflowInstance = workflowRuntime.CreateWorkflow(type);
            workflowInstance.Start();
            #endregion
        }


        #region WWF STUFFFF

        #region WWF Methodes
        public void DisplayLanguage()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("Latviešu");
                MainWindow.Items.Add("");
                MainWindow.Items.Add("Русский");
                MainWindow.Items.Add("");
                MainWindow.Items.Add("English");
            }), null);
        }
        public void DisplayAuthMenu()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("Enter Your Pin");
                MainWindow.Items.Add("");
            }), null);
        }
        public void ViewBalance(double accountAvailableBalance, double accountTotalBalance)
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("Your Available Balance is :"+accountAvailableBalance.ToString());
                MainWindow.Items.Add("You Have Total : " + accountTotalBalance.ToString());
                MainWindow.Items.Add("");
                MainWindow.Items.Add("Press 1 to Over this shit");
                MainWindow.Items.Add("Press 2 to Return to menu");
            }), null);
        }
        public void AuthFailed()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("Твой пин сосёд");
            }), null);
        }
        public void DisplayMenu()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("я менюшко");
                MainWindow.Items.Add("1 - View my balance");
                MainWindow.Items.Add("2 - Withdraw cash");
                MainWindow.Items.Add("3 - Deposit funds");
                MainWindow.Items.Add("4 - Exit");
                MainWindow.Items.Add("Choise your weapon");
            }), null);
        }
        public void SendBackMessage(string message)
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items[MainWindow.Items.Count-1] = message;
            }), null);
        }
        public void SendBackMessageLine(string message)
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Add(message);
            }), null);
        }
        public void WelcomeMessage()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                 MainWindow.Items.Add("Хелло мазафака введи карту");
                 MainWindow.Items.Add("");
             }), null);
        }
        public void NewPinCodeInvite()
        {
            this.Invoke(new EventHandler(delegate
            {
                MainWindow.Items.Clear();
                MainWindow.Items.Add("Введите новый пин");
                MainWindow.Items.Add("");
            }), null);
        }
        #endregion

        #region WWF events

        public event EventHandler<LanguageChoiseNumArgs> GetNewAction;
        public event EventHandler<GetNumArgs> GetNewNum;

        #endregion

        #region WWF EndPoint
        static void OnWorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            Console.WriteLine("Workflow completed; back to Form");
        }
        void workflowRuntime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            Console.WriteLine("Workflow terminated {0}", e.Exception);
        }
        #endregion


        #endregion

        #region UI StuffFFF
        public void GenerateButtons()
        {
            BMenu = new Button[3, 4];
            
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    Button tmp = new Button();
                    int g = i * 3 + j+1;
                    tmp.Name = g.ToString();
                    tmp.Text = g.ToString();
                    tmp.Size = new Size(20, 20);
                    tmp.Location = new Point((j * 20) + 3, (i * 20) + 3);
                    tmp.Click += new EventHandler(Num_Click);
                    tmp.FlatStyle = FlatStyle.Flat;
                    BMenu[i, j] = tmp;
                    MyPanel.Controls.Add(BMenu[i, j]);
                    

                }
            }
            Button LastBt = new Button();
            LastBt.Name = "0";
            LastBt.Text = "0";
            LastBt.Size = new Size(20, 20);
            LastBt.Location = new Point(23, 69);
            LastBt.Click += new EventHandler(Num_Click);
            LastBt.FlatStyle = FlatStyle.Flat;
            BMenu[2, 1] = LastBt;
            MyPanel.Controls.Add(BMenu[2, 1]);
        }
        public void GenerateAcctions()
        {
            BAction = new Button[5];
            for (int j = 0; j <=1; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    Button tmp = new Button();
                    int g = (3*j)+i + 1;
                    tmp.Name = g.ToString();
                    tmp.Text = g.ToString();
                    tmp.Size = new Size(20, 20);
                    tmp.Location = new Point(j*180, i * 23);
                    tmp.FlatStyle = FlatStyle.Flat;
                    tmp.Click += new EventHandler(Action_Click);
                    BAction[i] = tmp;

                    this.Controls.Add(BAction[i]);
                }
            }
        }
        void Action_Click(object sender, EventArgs e)
        {
            int id=Convert.ToInt32(((Button)sender).Text);
            LanguageChoiseNumArgs eventArgs=new LanguageChoiseNumArgs(workflowInstance.InstanceId,id);
            GetNewAction(null, eventArgs);
        }
        void Num_Click(object sender, EventArgs e)
        {
            string id=((Button)sender).Text;
            GetNumArgs eventArgs = new GetNumArgs(workflowInstance.InstanceId, id);
            GetNewNum(null, eventArgs);
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }




    }
}