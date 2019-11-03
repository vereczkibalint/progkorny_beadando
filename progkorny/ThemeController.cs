﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace progkorny
{
    public class ThemeController
    {
        /// <summary>
        /// Témaváltoztató metódus
        /// </summary>
        /// <param name="theme">Téma enum</param>
        public void ChangeTheme(Themes theme)
        {
            // App.xaml -> ThemeDictionary
            var ThemeDictionary = Application.Current.Resources.MergedDictionaries[4];
            Uri newTheme;

            // Ha a jelenlegi téma Light, akkor Dark kerül betöltésre
            if(theme == Themes.LIGHT)
            {
                newTheme = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml");
            }
            else
            {
                newTheme = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml");
            }

            // Kiürítjük a ThemeDirectory-t, és hozzáadjuk az új témát
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = newTheme });
        }

        /// <summary>
        /// Színváltoztatás
        /// </summary>
        /// <param name="color">Colors enum</param>
        public void ChangeColor(Colors color)
        {
            // App.xaml -> ColorSettingDictionary
            var ColorDictionary = Application.Current.Resources.MergedDictionaries[3];
            Uri newColor;

            // Színekhez tartozó xaml stílusok betöltése
            switch (color)
            {
                case Colors.KEK:
                    newColor = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml");
                    break;
                case Colors.NARANCSSARGA:
                    newColor = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Orange.xaml");
                    break;
                case Colors.LILA:
                    newColor = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Purple.xaml");
                    break;
                case Colors.BARNA:
                    newColor = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Brown.xaml");
                    break;
                default:
                    newColor = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml");
                    break;
            }

            // ColorSettingDictionary ürítése, majd új szín betöltése
            ColorDictionary.MergedDictionaries.Clear();
            ColorDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = newColor });

        }
    }
}
