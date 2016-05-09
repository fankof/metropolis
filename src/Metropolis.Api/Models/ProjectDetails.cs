﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Metropolis.Api.Extensions;

namespace Metropolis.Api.Models
{
    public class ProjectDetails : INotifyPropertyChanged
    {
        private string projectName;
        private string cSharpSourceDirectory;
        private string javaSourceDirectory;
        private string ecma6SourceDiredtory;

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                PropertyChanged.Notify(this, x => x.ProjectName);
            }
        }

        public string CSharpSourceDirectory
        {
            get { return cSharpSourceDirectory; }
            set
            {
                cSharpSourceDirectory = value;
                PropertyChanged.Notify(this, x => x.CSharpSourceDirectory);
            }
        }

        public string JavaSourceDirectory
        {
            get { return javaSourceDirectory; }
            set
            {
                javaSourceDirectory = value;
                PropertyChanged.Notify(this, x => x.JavaSourceDirectory);
            }
        }

        public string Ecma6SourceDiredtory
        {
            get { return ecma6SourceDiredtory; }
            set
            {
                ecma6SourceDiredtory = value;
                PropertyChanged.Notify(this, x => x.Ecma6SourceDiredtory);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}