using System;
using System.ComponentModel.DataAnnotations;
using MATS.Core.Common;
using System.Management;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Enumerations;

namespace MATS.Core.Models
{
    public class ProductActivationDTO : CommonFieldsA
    {
        public ProductActivationDTO()
        {
            BIOS_SN = Get_BIOS_SN();
        }

        [Required]
        [StringLength(150)]
        public string ProductKey
        {
            get { return GetValue(() => ProductKey); }
            set { SetValue(() => ProductKey, value); }
        }

        [Required]
        [StringLength(150)]
        public string LicensedTo
        {
            get { return GetValue(() => LicensedTo); }
            set { SetValue(() => LicensedTo, value); }
        }

        [Required]
        public int ClientId
        {
            get { return GetValue(() => ClientId); }
            set { SetValue(() => ClientId, value); }
        }

        [Required]
        public DateTime ActivatedDate
        {
            get { return GetValue(() => ActivatedDate); }
            set { SetValue(() => ActivatedDate, value); }
        }

        [Required]
        public DateTime ExpirationDate
        {
            get { return GetValue(() => ExpirationDate); }
            set { SetValue(() => ExpirationDate, value); }
        }

        [Required]
        [StringLength(150)]
        public string RegisteredBIOS_SN
        {
            get { return GetValue(() => RegisteredBIOS_SN); }
            set { SetValue(() => RegisteredBIOS_SN, value); }
        }

        [Required]
        [NotMapped]
        public string BIOS_SN
        {
            get { return GetValue(() => BIOS_SN); }
            set { SetValue(() => BIOS_SN, value); }
        }

        /// <summary>
        /// It discovers the BIOS serial number
        /// </summary>
        public static string Get_BIOS_SN()
        {
            string BIOSSN = string.Empty;

            //return Environment.MachineName;
            var searcher = new ManagementObjectSearcher("select SerialNumber from WIN32_BIOS");
            var result = searcher.Get();

            foreach (var o in result)
            {
                var obj = (ManagementObject)o;
                if (obj["SerialNumber"] != null)
                    BIOSSN = obj["SerialNumber"].ToString();
            }

            result.Dispose();
            searcher.Dispose();

            return BIOSSN;

        }
    }
}
