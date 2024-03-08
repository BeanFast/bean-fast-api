using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;

namespace Utilities.ValidationAttributes
{
    public class RequiredFileExtensionsAttribute : ValidationAttribute
    {
        private readonly AllowedFileTypes[] _allowedFileTypes;
        public RequiredFileExtensionsAttribute(params AllowedFileTypes[] AllowedFileTypes)
        {
            _allowedFileTypes = AllowedFileTypes;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is null) return ValidationResult.Success;
            bool result = false;
            HashSet<string> allowedExtension = new();
            using (var reader = new BinaryReader(file!.OpenReadStream()))
            {
                foreach (var item in _allowedFileTypes)
                {
                    Dictionary<string, List<byte[]>> fileSignatures = new();
                    switch (item)
                    {

                        case AllowedFileTypes.IMAGE:
                            {
                                fileSignatures = FileExtensionByteConstant.ImageFileSignatures;
                                
                                break;
                            }
                        case AllowedFileTypes.SOUND:
                            {
                                fileSignatures = FileExtensionByteConstant.SoundFileSignatures;
                                break;
                            }
                    }
                    var signatures = fileSignatures.Values.SelectMany(x => x).ToList();  // flatten all signatures to single list
                    var headerBytes = reader.ReadBytes(fileSignatures.Max(m => m.Value.Max(n => n.Length)));
                    result = result || signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                    fileSignatures.Keys.ToList().ForEach(x => allowedExtension.Add(x));
                }                
            }
            return result ? ValidationResult.Success : new ValidationResult(MessageConstants.FileMessageConstrant.FileExtensionsOnlyAccept(allowedExtension));
        }
    }
}
