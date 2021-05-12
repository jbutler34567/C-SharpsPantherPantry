using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualBasic;
//using C_SharpsPantherPantry.Logic;

/**
 * Author:  Jeff Butler
 * Date Created: 20010419
 * Description:  this form creates and sends a message to subscribers.  The messagebox is a richTextBox.  The toolbar wordprocessing controls
 * for that richTextBox.  There is a checkbox to insert all subscribers.  This pulls the email addresses from the database and populates the 
 * to: textbox.  
 * From is autopopulated from the person who logged in.
 * Send will prompt the user for a gmail login and send the message.  This will also log an entry in the database for the date of the message, who 
 * sent it, the subject, and the count of subscribers.
 * 
 * TODO:  create Send method,  popluate all subscribers, audit log for the message, auto populate the from textbox with the logged in user.
 * Todo:  clear the form when the send button is pressed.  completed.
 * TODO:  Set from to DoNotReply@pantherpantry.com
 * TODO:  Get userid of the logged in user
 * 
 * Modifications:  
 *JB 20210423  Send is now sending
 *JB 20210427  Added cancel Button.
 *JB 20210427  Clears form after sending and cancel button click event
 **/



namespace CSharpsPantherPantry_Jeff
{
    public partial class SendNotificationForm : Form
    {
        //the user for the session
        private int userID;

        public SendNotificationForm()
        {
            InitializeComponent();
            //this.userID = userID;
        }
        DB db = new DB();
        const string smtp = "smtp.gmail.com";
        const int port = 587;

        private void SendNotificationForm_Load(object sender, EventArgs e)
        {
            // populates image list
            appToolStrip.ImageList = ButtonImageList;
            closeToolStripButton.ImageIndex = 0;
            sendToolStripButton.ImageIndex = 1;
            openToolStripButton.ImageIndex = 2;
            copyToolStripButton.ImageIndex = 3;
            pasteToolStripButton.ImageIndex = 4;
            cutToolStripButton.ImageIndex = 5;
            undoToolStripButton.ImageIndex = 6;
            redoToolStripButton.ImageIndex = 7;
            leftToolStripButton.ImageIndex = 8;
            centerToolStripButton.ImageIndex = 9;
            rightToolStripButton.ImageIndex = 10;
            countToolStripButton.ImageIndex = 11;
        }
        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            //closes the form.  No save attion is performed at this time
            this.Close();
        }
    }
}
