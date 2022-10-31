using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

public class Record : MonoBehaviour
{
    public static float waitTime = 0.05f;
    private float waiting = 0f;

    public static int fileNum;

    public Text valueText;
    public Button recordBtn;
    public Text uuidText;

    public FileStream file;

    public bool isWriting = false;
    public bool isRecording = false;

    private int currentNum = 0;


    public void controlBtn()
    {
        var recordText = recordBtn.transform.Find("RecordText").GetComponent<Text>();
        if (recordText.text.Contains("Start"))
        {            
            isRecording = true;
            recordText.text = "Stop Recording";
            fileNum = fileNum + 1;
            currentNum = fileNum;
        }
        else
        {
            fileNum += 1;
            isRecording = false;
            recordText.text = "Start Recording";
            string fileName = "test" + currentNum + ".txt";
            sendMail(fileName);
            GameObject.Find("SendMailText").GetComponent<Text>().text = "File sent : " + fileName;
        }
    }

    public void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        if (isWriting)
        {
            Debug.LogError("currently writing");
            return;
        }
        string path = pathForDocumentsFile(filename);
        if (!File.Exists(path))
        {
            str = uuidText.text+"\n"+str;
        }

        using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            StreamWriter sw = new StreamWriter(file);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            isWriting = true;
            sw.WriteLine(str);
            Debug.LogError("writeString. filename : " + path + ", content : " + str);
            sw.Close();
            file.Close();
            isWriting = false;
        }
#endif
    }

    public void writeAuto()
    {
        string currentTime = DateTime.Now.ToString("hh.mm.ss.ffffff");
        string content = currentTime + "\t" + getValue();
        string fileName = "test" + currentNum.ToString() + ".txt";

        string path = pathForDocumentsFile(fileName);
        writeStringToFile(content, fileName);
    }


    public string readStringFromFile(string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadLine();

            sr.Close();
            file.Close();

            return str;
        }

        else
        {
            return null;
        }
#else
return null;
#endif
    }

    // 파일쓰고 읽는넘보다 이놈이 핵심이죠
    public string pathForDocumentsFile(string filename)
    {

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }







    // Start is called before the first frame update
    void Start()
    {
        recordBtn.onClick.AddListener(controlBtn);
        fileNum = checkFileNum();
    }

    // Update is called once per frame
    void Update()
    {
        float temp;
        if (float.TryParse(GameObject.Find("period").GetComponent<Text>().text, out temp))
        {
            waitTime = temp;
        }
        else
        {
            waitTime = 0.05f;
        }

        if (isRecording)
        {
            waiting += Time.deltaTime;
            if (waiting > waitTime)
            {
                string currentTime = DateTime.Now.ToString("hh.mm.ss.ffffff");
                string content = currentTime + "\t" + getValue();
                string fileName = "test" + currentNum.ToString() + ".txt";

                string path = pathForDocumentsFile(fileName);
                writeStringToFile(content, fileName);
                waiting = 0f;
            }
        }
    }


    public void sendMail(string fileName)
    {

        string toEmail=GameObject.Find("EmailText").GetComponent<Text>().text;
        if (toEmail.Length < 8)
        {
            toEmail = "wodn9955@daum.net";
        }

        MailMessage mail = new MailMessage();
        // 보내는 사람 메일, 이름, 인코딩(UTF-8)
        mail.From = new MailAddress("ts221move@gmail.com", "TS221", System.Text.Encoding.UTF8);
        // 받는 사람 메일
        mail.To.Add(toEmail);
        // 참조 사람 메일
        mail.CC.Add("wodn9955@daum.net");
        // 비공개 참조 사람 메일
        //mail.Bcc.Add("ts221move@gmail.com");
        // 메일 제목
        mail.Subject = "메일 제목";
        // 본문 내용
        mail.Body = "<html><body>hello wrold. This is for testing SMTP mail from GMAIL</body></html>";
        // 본문 내용 포멧의 타입 (true의 경우 Html 포멧으로)
        mail.IsBodyHtml = true;

        // 메일 제목과 본문의 인코딩 타입(UTF-8)
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.BodyEncoding = System.Text.Encoding.UTF8;


        // 첨부 파일 (Stream과 파일 이름)
        mail.Attachments.Add(new Attachment(new FileStream(pathForDocumentsFile(fileName), FileMode.Open, FileAccess.Read), fileName));

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;

        string pw = "pfjpraopwawlspxv";

#if UNITY_ANDROID && !UNITY_EDITOR
        {
            pw="izhvuxehkkxnhsyw";
        }
#endif

        smtpServer.Credentials = new System.Net.NetworkCredential("ts221move@gmail.com", pw);
        smtpServer.EnableSsl = true;
        //smtpServer.Send(mail);

        //Debug.Log("Email sending success");
        smtpServer.SendMailAsync(mail);
        smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
    }

    private void sendCallback(object sender, AsyncCompletedEventArgs e)
    {
        Debug.Log("Send Success");
    }


    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = "";// (string)e.UserState;


        if (e.Cancelled)
        {
            Debug.Log(token + " Send canceled.");
        }
        if (e.Error != null)
        {
            Debug.Log(token + ", " + e.Error.ToString());
        }
        else
        {
            Debug.LogError("Message sent.");
        }
    }



    private int checkFileNum()
    {
        bool exists = true;
        int i = 0;
        while (exists)
        {
            string path = pathForDocumentsFile("test" + i.ToString() + ".txt");
            exists = File.Exists(path);
            if (exists)
            {
                i = i + 1;
            }
            Debug.LogError(path + " exists : " + exists);
        }
        return i;
    }

    private int getValue()
    {
        if (valueText.text.Contains("No value"))
        {
            return -1;
        }

        int a = valueText.text.IndexOf(" : ") + 2;
        int b;
        if (Int32.TryParse(valueText.text.Substring(a), out b))
        {
            return b;
        }
        else
        {
            Debug.LogError("Parsing Error");
            return -1;
        }
    }
}