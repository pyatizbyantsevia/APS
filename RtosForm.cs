using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Test;
using System.Windows.Forms;
using RTOS_POSIX.RTOS;

namespace RTOS_POSIX
{
  public partial class RtosForm : Form
  {
    public RtosForm()
    {
      InitializeComponent()
    }

    /// <summary>
    /// Appends new string to output TextBox
    /// </summary>
    /// <param name="text"></param>
    delegate void AppendToOutputCallback(string text)
    ;
    public void AppendToOutput(string text)
    {
      if (this.outputTextBox.InvokeRequired) // if control in other thread
      {
        AppendToOutputCallback d = new AppendToOutputCallback(AppendToOutput); // Create callback delegate
        this.Invoke(d, new object[] { text }); // Call function from other thread
      }
      else // if control in current thread
      {
        outputTextBox.AppendText(text + "\t" + Environment.NewLine);
        outputTextBox.SelectionStart = outputTextBox.TextLength;
        outputTextBox.ScrollToCaret();
      }
    }

    /// <summary>
    /// Current _os instance
    /// </summary>
    OS _os = null

    private void startButton_Click(object sender, EventArgs e)
    {
      outputTextBox.AppendText("----------\n");
      _os = new OS(this);

      // Task 0
      System.Threading.ParametrizedThreadStart task0EntryPoint = new System.Threading.ParametrizedThreadStart( delegate(Object data) {
        OS.Print("Starting task 0");
        _os.TaskPool[1].Activate(_os.TaskPool[0]);
        if (_os.SemaphorePool[0].Signal())
          OS.Print("Task 0 Semaphore 0 signaled");
        if (_os.SemaphorePool[1].Signal())
          OS.Print("Task 0 Semaphore signaled");
        OS.Print("Terminating Task 0")
        _os.TaskPool[0].Terminate();
      });

      // Task 1 
      System.Threading.ParametrizedThreadStart task1EntryPoint = new System.Threading.ParametrizedThreadStart(delegate(Object data)
      {
        OS.Print("Starting task 1");
        _os.TaskPool[2].Activate(_os.TaskPool[1]);
        if (_os.SemaphorePool[0].Signal())
          OS.Print("Task 1 Semaphore 0 signaled");
        
        OS.Print("Task 1 waiting for Semaphore 1");
        _os.SemaphorePool[1].Wait(_os.TaskPool[1]);
        OS.Print("Terminating Task 1")
        _os.TaskPool[1].Terminate();
      });

      // Task 2
      System.Threading.ParametrizedThreadStart task2EntryPoint = new System.Threading.ParametrizedThreadStart(delegate(Object data)
      {
        OS.Print("Starting task 2");
        OS.Print("Task 2 wating for Semaphore 0");
        _os.SemaphorePool[0].Wait(_os.TaskPool[2]);
        if (_os.SemaphorePool[1].Signal())
          OS.Pring("Task 2 Semaphore 1 signaled");
        OS.Print("Terminating Task 2");
        _os.TaskPool[2].Terminate();
      });

      _os.TaskPool.Add(new Task(task0EntryPoint, "Task-0", 13));
      _os.TaskPool.Add(new Task(task1EntryPoint, "Task-1", 13));
      _os.TaskPool.Add(new Task(task2EntryPoint, "Task-2", 13));

      _os.SemaphorePool.Add(new Semaphore(0, 1));
      _os.SemaphorePool.Add(new Semaphore(0, 1));

      _os.Start()
    }

    private void stopButtong_Click(object sender, EventArgs e)
    {
      _os.Shutdown();
    }

  }
}