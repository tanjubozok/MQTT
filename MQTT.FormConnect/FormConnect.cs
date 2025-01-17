using MQTT.FormConnect.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTT.FormConnect
{
    public partial class FormConnect : Form
    {
        private MqttService mqttService;
        private List<FormMessaging> messagingForms;

        public FormConnect()
        {
            InitializeComponent();
            messagingForms = new List<FormMessaging>();
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            string brokerIp = "broker.emqx.io";
            string clientId = "a76a13b7-7ce5-47b5-ac4e-d06eca4fcea3";

            mqttService = new MqttService();
            await mqttService.Start(brokerIp, clientId, UpdateStatus);
        }

        private void newFormButton_Click(object sender, EventArgs e)
        {
            FormMessaging formMessaging = new FormMessaging(mqttService, this);
            messagingForms.Add(formMessaging);
            formMessaging.Show();
        }

        public void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
            }
            else
            {
                statusLabel.Text = message;
                messagesListBox.Items.Add(message);
                foreach (var form in messagingForms)
                {
                    form.UpdateMessages(message);
                }
            }
        }

        public void SendMessageToOtherForms(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SendMessageToOtherForms), message);
            }
            else
            {
                messagesListBox.Items.Add(message);
                foreach (var form in messagingForms)
                {
                    form.UpdateMessages(message);
                }
            }
        }
    }
}
