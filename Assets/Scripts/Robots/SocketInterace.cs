using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class SocketInterace : MonoBehaviour
{
	
	public static SocketInterace Instance;
	
	public string ip = "localhost";
	public bool started;
	public DebugRover rover;
	public RenderTexture frame;
	public Material testFrame;
	public int bufferSize = 8192;//8192;
	public int headerSize = 5;
	
	public Socket client;
	
    // Start is called before the first frame update
    void Start()
    {
		
		Instance = this;
		
    }
	
	public void Connect() {
		
		IPHostEntry host;
		IPAddress ipAddress;
		if(ip == "localhost") {
			host = Dns.GetHostEntry(ip);
			ipAddress = host.AddressList[1];
		}
		else {
			host = Dns.GetHostEntry(ip);
			ipAddress = host.AddressList[0];
		}
		
		IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9001);
		
        client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
				
		client.SendBufferSize = bufferSize;
		
		client.Connect(remoteEP);
		Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());
		started = true;
		
	}
	
	private float frm = 0;

    // Update is called once per frame
    void Update()
    {
		
		int dataSize = bufferSize - headerSize;
		
		int chunkId = 0;
        
		if(frm < 5 && rover != null) {
			frm += Time.deltaTime;
			return;
		}
		
		int res = 512;
		int imgSize = res*res*3;
		
		RenderTexture.active = frame;
		Texture2D tex = new Texture2D(res, res, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, res, res), 0, 0);
		tex.Apply();
		
		
		
		if(started) {
			
			rover.netControlled = true;
			
			string output = "";
			output += rover.latitude;
			output += " ";
			output += rover.longitude;
			output += " ";
			output += rover.heading;
			output += " ";
			output += rover.vel;
			output += " ";
			//output += rover.currentWaypoint.x;
			output += rover.currentWaypointLATLNG.x;
			output += " ";
			//output += rover.currentWaypoint.y;
			output += rover.currentWaypointLATLNG.y;
			output += " ";
			output += imgSize;
			output += "\n";
			
			int msgSize = output.Length;
			byte[] intBytes = BitConverter.GetBytes(msgSize);
			output = "" + (char)0 + intBytes[0] + intBytes[1] + intBytes[2] + intBytes[3] + output;
			
			byte[] messageSent = Encoding.ASCII.GetBytes(output);
			int byteSent = client.Send(messageSent);
			//Debug.Log("PosData: " + output);
			
			byte[] messageRecv = new byte[bufferSize];
			client.Receive(messageRecv, 0, bufferSize, SocketFlags.None);
			var response = Encoding.UTF8.GetString(messageRecv);
			//Debug.Log("Got: " + response);
			
			
			//testFrame.SetTexture("_MainTex", tex);
			
			byte[] imgBytes = new byte[imgSize];
			int bPos = 0;
			for(int y = 0; y < res; y++) {
				for(int x = 0; x < res; x++) {
					for(int c = 0; c < 3; c++) {
						
						Color val = tex.GetPixel(x, y);
						int amt = (int)(255*val[c]);
						//Debug.Log(val);
						imgBytes[bPos] = (byte)amt;
						
						bPos++;
					}
				}
			}
			
			byteSent = client.Send(imgBytes);
			messageRecv = new byte[bufferSize];
			client.Receive(messageRecv, 0, bufferSize, SocketFlags.None);
			response = Encoding.UTF8.GetString(messageRecv);
			Debug.Log("Got: " + response);
			
			string[] dats = response.Split(' ');
			
			float forward = float.Parse(dats[0]);
			float turn = float.Parse(dats[1]);
			float lightInp = float.Parse(dats[2]);
			
			rover.netForwardInput = forward;
			rover.netTurnInput = turn;
			rover.netLightInput = lightInp;
			
			
		}
		
    }
}
