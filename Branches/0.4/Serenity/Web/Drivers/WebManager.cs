/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Manages WebDrivers and provides methods to control them.
    /// </summary>
    public static class WebManager
    {
        private static List<WebDriver> drivers = new List<WebDriver>();

        public static T GetDriver<T>() where T : WebDriver
        {
            foreach (WebDriver driver in WebManager.drivers)
            {
                if (driver is T)
                {
                    return (T)driver;
                }
            }
            return null;
        }
        public static void AddDriver(WebDriver newDriver)
        {
            foreach (WebDriver driver in WebManager.drivers)
            {
                if (driver.GetType() == newDriver.GetType())
                {
                    return;
                }
            }
            WebManager.drivers.Add(newDriver);
        }
        public static void Initialize<T>(WebDriverSettings settings) where T : WebDriver
        {
            T driver = WebManager.GetDriver<T>();
            if (driver != null)
            {
                lock (driver)
                {
                    driver.Initialize(settings);
                }
            }
        }
        public static void Stop<T>() where T : WebDriver
        {
            T driver = WebManager.GetDriver<T>();
            if (driver != null)
            {
                lock (driver)
                {
                    driver.Stop();
                }
            }
        }
        public static void Start<T>() where T : WebDriver
        {
            WebManager.Start<T>(false);
        }
        public static void Start<T>(bool threaded) where T : WebDriver
        {
            T driver = WebManager.GetDriver<T>();
            if (driver != null)
            {
                lock (driver)
                {
                    if ((driver.State > WebDriverState.Initialized) && (driver.State < WebDriverState.Starting))
                    {
                        if (threaded == true)
                        {
                            driver.ThreadedStart();
                        }
                        else
                        {
                            driver.Start();
                        }
                    }
                }
            }
        }
        public static void StopAll()
        {
            foreach (WebDriver driver in WebManager.drivers)
            {
                lock (driver)
                {
                    if (driver.State > WebDriverState.Starting)
                    {
                        driver.Stop();
                    }
                }
            }
        }
        public static void StartAll()
        {
            WebManager.StartAll(false);
        }
        public static void StartAll(bool threaded)
        {
            List<WebDriver> initializedDrivers = new List<WebDriver>();
            foreach (WebDriver driver in WebManager.drivers)
            {
                lock (driver)
                {
                    if ((driver.State >= WebDriverState.Initialized) && (driver.State < WebDriverState.Starting))
                    {
                        initializedDrivers.Add(driver);
                    }
                }
            }
            if (threaded == true)
            {
                for (int i = 0; i < initializedDrivers.Count; i++)
                {
                    initializedDrivers[i].ThreadedStart();
                }
            }
            else
            {
                for (int i = 0; i < initializedDrivers.Count - 1; i++)
                {
                    initializedDrivers[i].ThreadedStart();
                }
                initializedDrivers[initializedDrivers.Count - 1].Start();
            }
        }
    }
}
