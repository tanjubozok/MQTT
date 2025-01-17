using MQTT.FormConnect.Services;
using System;
using System.Windows.Forms;

namespace MQTT.FormConnect
{
    public partial class FormMessaging : Form
    {
        private MqttService mqttService;
        private FormConnect formConnect;

        public FormMessaging(MqttService mqttService, FormConnect formConnect)
        {
            InitializeComponent();
            this.mqttService = mqttService;
            this.formConnect = formConnect;
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            string message = messageTextBox.Text;
            await mqttService.SendCode(message);
            UpdateMessages($"Sent: {message}");
            formConnect.SendMessageToOtherForms($"Sent: {message}");
        }

        public void UpdateMessages(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateMessages), message);
            }
            else
            {
                messageListView.Items.Add(message);
            }
        }
    }
}
