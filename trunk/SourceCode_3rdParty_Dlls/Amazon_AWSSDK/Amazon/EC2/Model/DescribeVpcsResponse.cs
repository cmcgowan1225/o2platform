﻿namespace Amazon.EC2.Model
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRoot(Namespace="http://ec2.amazonaws.com/doc/2009-11-30/", IsNullable=false)]
    public class DescribeVpcsResponse
    {
        private Amazon.EC2.Model.DescribeVpcsResult describeVpcsResultField;
        private Amazon.EC2.Model.ResponseMetadata responseMetadataField;

        public bool IsSetDescribeVpcsResult()
        {
            return (this.describeVpcsResultField != null);
        }

        public bool IsSetResponseMetadata()
        {
            return (this.responseMetadataField != null);
        }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder(0x400);
            XmlSerializer serializer = new XmlSerializer(base.GetType());
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize((TextWriter) writer, this);
            }
            return sb.ToString();
        }

        public DescribeVpcsResponse WithDescribeVpcsResult(Amazon.EC2.Model.DescribeVpcsResult describeVpcsResult)
        {
            this.describeVpcsResultField = describeVpcsResult;
            return this;
        }

        public DescribeVpcsResponse WithResponseMetadata(Amazon.EC2.Model.ResponseMetadata responseMetadata)
        {
            this.responseMetadataField = responseMetadata;
            return this;
        }

        [XmlElement(ElementName="DescribeVpcsResult")]
        public Amazon.EC2.Model.DescribeVpcsResult DescribeVpcsResult
        {
            get
            {
                return this.describeVpcsResultField;
            }
            set
            {
                this.describeVpcsResultField = value;
            }
        }

        [XmlElement(ElementName="ResponseMetadata")]
        public Amazon.EC2.Model.ResponseMetadata ResponseMetadata
        {
            get
            {
                return this.responseMetadataField;
            }
            set
            {
                this.responseMetadataField = value;
            }
        }
    }
}

