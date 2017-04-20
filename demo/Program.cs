using System;
using System.Diagnostics;
using System.Threading;
using XLDownload;

namespace Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ok = XL.Init();
            Debug.Assert(ok);

            // 下载限速
            XL.SetSpeedLimit(500);

            // 上传限速
            XL.SetUploadSpeedLimit(100, 100);

            Test1();
            Test2();

            Console.WriteLine("End");
            Console.ReadLine();

            ok = XL.UnInit();
            Debug.Assert(ok);
        }

        /// <summary>
        /// 使用API方式下载，停止，删除
        /// </summary>
        private static void Test1()
        {
            // 建立任务
            var param = new XL.DownTaskParam()
            {
                TaskUrl = "https://down5.huorong.cn/sysdiag-all-4.0.19.4.exe",
                SavePath = @".\",
                FileName = "test1.exe",
            };
            var task = XL.CreateTask(param);
            Debug.Assert(task != null);

            // 启动任务
            var ok = XL.StartTask(task);
            Debug.Assert(ok);

            // 下载5秒
            Thread.Sleep(5000);

            // 停止任务
            ok = XL.StopTask(task);
            Debug.Assert(ok);

            // 等待任务完全停止
            var taskInfo = new XL.TaskInfo();
            while (XL.QueryTaskInfoEx(task, taskInfo))
            {
                Console.WriteLine(taskInfo.State);
                if (taskInfo.State == XL.TaskStatus.Pause)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            // 移除任务
            ok = XL.DeleteTask(task);
            Debug.Assert(ok);

            // 清理临时文件
            ok = XL.DelTempFile(param);
            Debug.Assert(ok);
        }

        private static void Test2()
        {
            // 建立任务
            var param = new XL.DownTaskParam()
            {
                TaskUrl = "https://down5.huorong.cn/sysdiag-all-4.0.19.4.exe",
                SavePath = @".\",
                FileName = "test2.exe",
            };
            var task = XL.CreateTask(param);
            Debug.Assert(task != null);

            // 启动任务
            var ok = XL.StartTask(task);
            Debug.Assert(ok);

            // 下载5秒
            Thread.Sleep(5000);

            // 停止任务
            ok = XL.StopTask(task);
            Debug.Assert(ok);

            // 等待任务完全停止
            var taskInfo = new XL.TaskInfo();
            while (XL.QueryTaskInfoEx(task, taskInfo))
            {
                Console.WriteLine(taskInfo.State);
                if (taskInfo.State == XL.TaskStatus.Pause)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            // 移除任务
            ok = XL.DeleteTask(task);
            Debug.Assert(ok);

            // 续传任务
            task = XL.CreateTask(param);
            Debug.Assert(task != null);
            ok = XL.StartTask(task);
            Debug.Assert(ok);

            // 等待下载完成
            while (XL.QueryTaskInfoEx(task, taskInfo))
            {
                if (taskInfo.State == XL.TaskStatus.Download)
                {
                    Console.WriteLine(taskInfo.Percent);
                }
                else
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            // 释放任务
            ok = XL.DeleteTask(task);
            Debug.Assert(ok);
        }
    }
}