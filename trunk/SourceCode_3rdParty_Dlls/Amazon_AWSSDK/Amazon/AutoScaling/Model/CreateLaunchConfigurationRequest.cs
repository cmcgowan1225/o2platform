﻿namespace Amazon.AutoScaling.Model
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(Namespace="http://autoscaling.amazonaws.com/doc/2009-05-15/", IsNullable=false)]
    public class CreateLaunchConfigurationRequest
    {
        private List<BlockDeviceMapping> blockDeviceMappingsField;
        private string imageIdField;
        private string instanceTypeField;
        private string kernelIdField;
        private string keyNameField;
        private string launchConfigurationNameField;
        private string ramdiskIdField;
        private List<string> securityGroupsField;
        private string userDataField;

        public bool IsSetBlockDeviceMappings()
        {
            return (this.BlockDeviceMappings.Count > 0);
        }

        public bool IsSetImageId()
        {
            return (this.imageIdField != null);
        }

        public bool IsSetInstanceType()
        {
            return (this.instanceTypeField != null);
        }

        public bool IsSetKernelId()
        {
            return (this.kernelIdField != null);
        }

        public bool IsSetKeyName()
        {
            return (this.keyNameField != null);
        }

        public bool IsSetLaunchConfigurationName()
        {
            return (this.launchConfigurationNameField != null);
        }

        public bool IsSetRamdiskId()
        {
            return (this.ramdiskIdField != null);
        }

        public bool IsSetSecurityGroups()
        {
            return (this.SecurityGroups.Count > 0);
        }

        public bool IsSetUserData()
        {
            return (this.userDataField != null);
        }

        public CreateLaunchConfigurationRequest WithBlockDeviceMappings(params BlockDeviceMapping[] list)
        {
            foreach (BlockDeviceMapping mapping in list)
            {
                this.BlockDeviceMappings.Add(mapping);
            }
            return this;
        }

        public CreateLaunchConfigurationRequest WithImageId(string imageId)
        {
            this.imageIdField = imageId;
            return this;
        }

        public CreateLaunchConfigurationRequest WithInstanceType(string instanceType)
        {
            this.instanceTypeField = instanceType;
            return this;
        }

        public CreateLaunchConfigurationRequest WithKernelId(string kernelId)
        {
            this.kernelIdField = kernelId;
            return this;
        }

        public CreateLaunchConfigurationRequest WithKeyName(string keyName)
        {
            this.keyNameField = keyName;
            return this;
        }

        public CreateLaunchConfigurationRequest WithLaunchConfigurationName(string launchConfigurationName)
        {
            this.launchConfigurationNameField = launchConfigurationName;
            return this;
        }

        public CreateLaunchConfigurationRequest WithRamdiskId(string ramdiskId)
        {
            this.ramdiskIdField = ramdiskId;
            return this;
        }

        public CreateLaunchConfigurationRequest WithSecurityGroups(params string[] list)
        {
            foreach (string str in list)
            {
                this.SecurityGroups.Add(str);
            }
            return this;
        }

        public CreateLaunchConfigurationRequest WithUserData(string userData)
        {
            this.userDataField = userData;
            return this;
        }

        [XmlElement(ElementName="BlockDeviceMappings")]
        public List<BlockDeviceMapping> BlockDeviceMappings
        {
            get
            {
                if (this.blockDeviceMappingsField == null)
                {
                    this.blockDeviceMappingsField = new List<BlockDeviceMapping>();
                }
                return this.blockDeviceMappingsField;
            }
            set
            {
                this.blockDeviceMappingsField = value;
            }
        }

        [XmlElement(ElementName="ImageId")]
        public string ImageId
        {
            get
            {
                return this.imageIdField;
            }
            set
            {
                this.imageIdField = value;
            }
        }

        [XmlElement(ElementName="InstanceType")]
        public string InstanceType
        {
            get
            {
                return this.instanceTypeField;
            }
            set
            {
                this.instanceTypeField = value;
            }
        }

        [XmlElement(ElementName="KernelId")]
        public string KernelId
        {
            get
            {
                return this.kernelIdField;
            }
            set
            {
                this.kernelIdField = value;
            }
        }

        [XmlElement(ElementName="KeyName")]
        public string KeyName
        {
            get
            {
                return this.keyNameField;
            }
            set
            {
                this.keyNameField = value;
            }
        }

        [XmlElement(ElementName="LaunchConfigurationName")]
        public string LaunchConfigurationName
        {
            get
            {
                return this.launchConfigurationNameField;
            }
            set
            {
                this.launchConfigurationNameField = value;
            }
        }

        [XmlElement(ElementName="RamdiskId")]
        public string RamdiskId
        {
            get
            {
                return this.ramdiskIdField;
            }
            set
            {
                this.ramdiskIdField = value;
            }
        }

        [XmlElement(ElementName="SecurityGroups")]
        public List<string> SecurityGroups
        {
            get
            {
                if (this.securityGroupsField == null)
                {
                    this.securityGroupsField = new List<string>();
                }
                return this.securityGroupsField;
            }
            set
            {
                this.securityGroupsField = value;
            }
        }

        [XmlElement(ElementName="UserData")]
        public string UserData
        {
            get
            {
                return this.userDataField;
            }
            set
            {
                this.userDataField = value;
            }
        }
    }
}

