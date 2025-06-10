/*using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Services.DataInitializer
{
    public class RealEstateDataInitializer : IDataInitializer
    {
        private readonly IRepository<RealEstateInfoFile> _realEstateRepository;
        private readonly IWebHostEnvironment _env;

        public RealEstateDataInitializer    (IRepository<RealEstateInfoFile> realEstateRepository, IWebHostEnvironment env)
        {
            _realEstateRepository = realEstateRepository;
            _env = env;
        }

        public void InitializeData()
        {
            if (_realEstateRepository.TableNoTracking.Any())
            {
                return; // اگه رکورد داره، کاری نکن
            }
            var path = Path.Combine(_env.ContentRootPath, "wwwroot", "files", "RealEstateInfoFiles.csv");
            if (!File.Exists(path))
                throw new FileNotFoundException("Real Estate CSV file not found", path);

            var csvContent = File.ReadAllText(path, Encoding.UTF8);
            var lines = csvContent.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            if (lines.Count < 2) return; // باید حداقل header و یک ردیف داده داشته باشیم

            // حذف هدر (دو خط اول چون header تکرار شده)
            var dataLines = lines.Skip(2).ToList();

            List<RealEstateInfoFile> realEstateList = new List<RealEstateInfoFile>();

            foreach (var line in dataLines)
            {
                try
                {
                    var fields = ParseCsvLine(line);
                    if (fields.Length < 10) continue; // حداقل فیلدهای ضروری

                    var realEstate = new RealEstateInfoFile
                    {
                        Id = Guid.NewGuid(),
                        Mantaghe = GetStringValue(fields, 0),
                        TotalFloors = GetIntValue(fields, 1),
                        UnitsPerFloor = GetIntValue(fields, 2),
                        TotalPrice = GetDecimalValue(fields, 3),
                        PricePerSquareMeter = GetDecimalValue(fields, 4),
                        Floors = GetIntValue(fields, 5),
                        BuiltArea = GetIntValue(fields, 6),
                        Bedrooms = GetIntValue(fields, 7),
                        Balconies = GetIntValue(fields, 8),
                        HasTelephone = GetBoolValue(fields, 9),
                        KitchenType = ParseKitchenType(GetStringValue(fields, 10)),
                        IsKitchenOpen = GetBoolValue(fields, 11),
                        BathroomType = ParseBathroomType(GetStringValue(fields, 12)),
                        FlooringType = ParseFlooringType(GetStringValue(fields, 13)),
                        HasParking = GetBoolValue(fields, 14),
                        HasStorage = GetBoolValue(fields, 15),
                        BuildingAge = GetStringValue(fields, 16),
                        FacadeType = ParseFacadeType(GetStringValue(fields, 17)),
                        DocumentType = ParseDocumentType(GetStringValue(fields, 18)),
                        HasGas = GetBoolValue(fields, 19),
                        HasElevator = GetBoolValue(fields, 20),
                        HasVideoIntercom = GetBoolValue(fields, 21),
                        HasCooler = GetBoolValue(fields, 22),
                        HasRemoteControlDoor = GetBoolValue(fields, 23),
                        HasRadiator = GetBoolValue(fields, 24),
                        HasPackageHeater = GetBoolValue(fields, 25),
                        IsRenovated = GetBoolValue(fields, 26),
                        HasJacuzzi = GetBoolValue(fields, 27),
                        HasSauna = GetBoolValue(fields, 28),
                        HasPool = GetBoolValue(fields, 29),
                        HasLobby = GetBoolValue(fields, 30),
                        HasDuctSplit = GetBoolValue(fields, 31),
                        HasChiller = GetBoolValue(fields, 32),
                        HasRoofGarden = GetBoolValue(fields, 33),
                        HasMasterRoom = GetBoolValue(fields, 34),
                        HasNoElevator = GetBoolValue(fields, 35),
                        HasSplitAC = GetBoolValue(fields, 36),
                        HasJanitorService = GetBoolValue(fields, 37),
                        HasMeetingHall = GetBoolValue(fields, 38),
                        HasFanCoil = GetBoolValue(fields, 39),
                        HasGym = GetBoolValue(fields, 40),
                        HasCCTV = GetBoolValue(fields, 41),
                        HasEmergencyPower = GetBoolValue(fields, 42),
                        IsFlat = GetBoolValue(fields, 43),
                        HasUnderfloorHeating = GetBoolValue(fields, 44),
                        HasFireAlarm = GetBoolValue(fields, 45),
                        HasFireExtinguishingSystem = GetBoolValue(fields, 46),
                        HasCentralVacuum = GetBoolValue(fields, 47),
                        HasSecurityDoor = GetBoolValue(fields, 48),
                        HasLaundry = GetBoolValue(fields, 49),
                        IsFurnished = GetBoolValue(fields, 50),
                        IsSoldWithTenant = GetBoolValue(fields, 51),
                        HasCentralAntenna = GetBoolValue(fields, 52),
                        HasBackyard = GetBoolValue(fields, 53),
                        HasServantService = GetBoolValue(fields, 54),
                        HasBarbecue = GetBoolValue(fields, 55),
                        HasElectricalPanel = GetBoolValue(fields, 56),
                        HasConferenceHall = GetBoolValue(fields, 57),
                        HasGuardService = GetBoolValue(fields, 58),
                        HasAirHandler = GetBoolValue(fields, 59),
                        HasCentralSatellite = GetBoolValue(fields, 60),
                        HasGarbageChute = GetBoolValue(fields, 61),
                        HasLobbyManService = GetBoolValue(fields, 62),
                        HasGuardOrJanitorService = GetBoolValue(fields, 63)
                    };

                    realEstateList.Add(realEstate);
                }
                catch (Exception ex)
                {
                    // لاگ خطا و ادامه به ردیف بعدی
                    Console.WriteLine($"Error processing line: {line}. Error: {ex.Message}");
                    continue;
                }
            }

            CancellationToken cancellationToken= new CancellationToken();
            if (realEstateList.Any())
            {
                _realEstateRepository.AddRangeAsync(realEstateList,cancellationToken).GetAwaiter().GetResult();
            }
        }

        // متدهای کمکی برای پارس کردن داده‌ها
        private string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == '\t' && !inQuotes) // فایل CSV با Tab جدا شده
                {
                    fields.Add(current.ToString().Trim());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString().Trim());
            return fields.ToArray();
        }

        private string GetStringValue(string[] fields, int index)
        {
            if (index >= fields.Length) return string.Empty;
            var value = fields[index]?.Trim();
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        private int GetIntValue(string[] fields, int index)
        {
            if (index >= fields.Length) return 0;
            var value = fields[index]?.Trim();
            if (string.IsNullOrEmpty(value)) return 0;

            if (int.TryParse(value, out int result))
                return result;

            return 0;
        }

        private decimal GetDecimalValue(string[] fields, int index)
        {
            if (index >= fields.Length) return 0;
            var value = fields[index]?.Trim();
            if (string.IsNullOrEmpty(value)) return 0;

            // پردازش نوتیشن علمی مثل 7.15E+13
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                return result;

            return 0;
        }

        private bool GetBoolValue(string[] fields, int index)
        {
            if (index >= fields.Length) return false;
            var value = fields[index]?.Trim();
            if (string.IsNullOrEmpty(value)) return false;

            return value == "1" || value.ToLower() == "true";
        }

        // پارس کردن نوع آشپزخانه
        private KitchenType ParseKitchenType(string value)
        {
            if (string.IsNullOrEmpty(value)) return KitchenType.Unknown;

            return value.ToLower().Trim() switch
            {
                "mdf" => KitchenType.MDF,
                "membrane" => KitchenType.Membrane,
                "furnished" => KitchenType.Furnished,
                "high-class" => KitchenType.HighClass,
                "custom" => KitchenType.Custom,
                "neo-classic" => KitchenType.NeoClassic,
                "wooden" => KitchenType.Wooden,
                "classic" => KitchenType.Classic,
                "german-pvc" => KitchenType.GermanPVC,
                "polished" => KitchenType.Polished,
                "metallic" => KitchenType.Metallic,
                "wood-metal" => KitchenType.WoodMetal,
                "modern" => KitchenType.Modern,
                "pantry" => KitchenType.Pantry,
                _ => KitchenType.Unknown
            };
        }

        // پارس کردن نوع سرویس بهداشتی
        private BathroomType ParseBathroomType(string value)
        {
            if (string.IsNullOrEmpty(value)) return BathroomType.Persian;

            return value.ToLower().Trim() switch
            {
                "western" => BathroomType.Western,
                "persian" => BathroomType.Persian,
                "persian-western" => BathroomType.PersianWestern,
                "public" => BathroomType.Public,
                "private" => BathroomType.Private,
                _ => BathroomType.Persian
            };
        }

        // پارس کردن نوع کف‌پوش
        private FlooringType ParseFlooringType(string value)
        {
            if (string.IsNullOrEmpty(value)) return FlooringType.Unknown;

            return value.ToLower().Trim() switch
            {
                "stone" => FlooringType.Stone,
                "ceramic" => FlooringType.Ceramic,
                "parquet" => FlooringType.Parquet,
                "laminate" => FlooringType.Laminate,
                "ceramic parquet" => FlooringType.CeramicParquet,
                "stone parquet" => FlooringType.StoneParquet,
                "carpet" => FlooringType.Carpet,
                "mosaic" => FlooringType.Mosaic,
                "mosaic stone" => FlooringType.MosaicStone,
                "wooden" => FlooringType.Wooden,
                "pvc" => FlooringType.PVC,
                _ => FlooringType.Unknown
            };
        }

        // پارس کردن نوع نما
        private FacadeType ParseFacadeType(string value)
        {
            if (string.IsNullOrEmpty(value)) return FacadeType.Unknown;

            return value.ToLower().Trim() switch
            {
                "stone" => FacadeType.Stone,
                "modern stone" => FacadeType.ModernStone,
                "roman stone" => FacadeType.RomanStone,
                "roman" => FacadeType.Roman,
                "classic" => FacadeType.Classic,
                "brick" => FacadeType.Brick,
                "3cm brick" => FacadeType.ThreeCmBrick, // برای "3cm Brick" در اکسل
                "Stone & Brick" => FacadeType.StoneAndBrick,
                "stone & glass" => FacadeType.StoneAndGlass,
                "cement" => FacadeType.Cement,
                "cement stone" => FacadeType.CementStone,
                "glass" => FacadeType.Glass,
                "ceramic" => FacadeType.Ceramic,
                "thermo wood" => FacadeType.ThermoWood,
                "granite" => FacadeType.Granite,
                "composite" => FacadeType.Composite,
                "composite panel" => FacadeType.CompositePanel,
                _ => FacadeType.Unknown
            };
        }

        private DocumentType ParseDocumentType(string value)
        {
            if (string.IsNullOrEmpty(value)) return DocumentType.Unknown;

            return value.ToLower().Trim() switch
            {
                "personal" => DocumentType.Personal,
                "residential" => DocumentType.Residential,
                "endowment" => DocumentType.Endowment,
                "promissory" => DocumentType.Promissory,
                "administrative" => DocumentType.Administrative,
                "commercial" => DocumentType.Commercial,
                "guidance endowment" => DocumentType.GuidanceEndowment,
                "cooperative" => DocumentType.Cooperative,
                _ => DocumentType.Unknown
            };
        }
    }
}
*/
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services.DataInitializer
{
    public class RealEstateDataInitializer : IDataInitializer
    {
        private readonly IRepository<RealEstateInfoFile> _realEstateRepository;
        private readonly IWebHostEnvironment _env;

        public RealEstateDataInitializer(IRepository<RealEstateInfoFile> realEstateRepository, IWebHostEnvironment env)
        {
            _realEstateRepository = realEstateRepository;
            _env = env;
        }

        public void InitializeData()
        {
            if (_realEstateRepository.TableNoTracking.Any())
            {
                return; // اگه رکورد داره، کاری نکن
            }

            var path = Path.Combine(_env.ContentRootPath, "wwwroot", "files", "RealEstateInfoFiles.csv");
            if (!File.Exists(path))
                throw new FileNotFoundException("Real Estate CSV file not found", path);

            try
            {
                // خواندن فایل با UTF-8 encoding
                var csvContent = File.ReadAllText(path, Encoding.UTF8);
                var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                Console.WriteLine($"Total lines read: {lines.Count}");

                if (lines.Count < 2)
                {
                    Console.WriteLine("CSV file doesn't have enough data rows");
                    return;
                }

                // نمایش خط اول (header) برای debug
                Console.WriteLine($"Header line: {lines[0]}");

                // نمایش خط دوم (اولین داده) برای debug
                if (lines.Count > 1)
                {
                    Console.WriteLine($"First data line: {lines[1]}");
                    var testFields = ParseCsvLine(lines[1]);
                    Console.WriteLine($"First data line parsed into {testFields.Length} fields");
                }

                // فقط یک خط header را حذف کن
                var dataLines = lines.Skip(1).ToList();

                Console.WriteLine($"Processing {dataLines.Count} data rows from CSV");

                List<RealEstateInfoFile> realEstateList = new List<RealEstateInfoFile>();

                for (int i = 0; i < dataLines.Count; i++)
                {
                    try
                    {
                        var line = dataLines[i];
                        var fields = ParseCsvLine(line);

                        Console.WriteLine($"Row {i + 1}: Found {fields.Length} fields");

                        if (fields.Length < 20) // کاهش حد آستانه برای تست
                        {
                            Console.WriteLine($"Row {i + 1}: Not enough fields ({fields.Length}), expected at least 20");

                            // برای debug، چند فیلد اول را نمایش بده
                            for (int j = 0; j < Math.Min(fields.Length, 5); j++)
                            {
                                Console.WriteLine($"  Field {j}: '{fields[j]}'");
                            }
                            continue;
                        }

                        var realEstate = new RealEstateInfoFile
                        {
                            Id = Guid.NewGuid(),
                            Mantaghe = GetStringValue(fields, 0),
                            TotalFloors = GetIntValue(fields, 1),
                            UnitsPerFloor = GetIntValue(fields, 2),
                            TotalPrice = GetDecimalValue(fields, 3),
                            PricePerSquareMeter = GetDecimalValue(fields, 4),
                            Floors = GetIntValue(fields, 5),
                            BuiltArea = GetIntValue(fields, 6),
                            Bedrooms = GetIntValue(fields, 7),
                            Balconies = GetIntValue(fields, 8),
                            HasTelephone = GetBoolValue(fields, 9),
                            KitchenType = ParseKitchenType(GetStringValue(fields, 10)),
                            IsKitchenOpen = GetBoolValue(fields, 11),
                            BathroomType = ParseBathroomType(GetStringValue(fields, 12)),
                            FlooringType = ParseFlooringType(GetStringValue(fields, 13)),
                            HasParking = GetBoolValue(fields, 14),
                            HasStorage = GetBoolValue(fields, 15),
                            BuildingAge = GetStringValue(fields, 16),
                            FacadeType = ParseFacadeType(GetStringValue(fields, 17)),
                            DocumentType = ParseDocumentType(GetStringValue(fields, 18)),
                            HasGas = GetBoolValue(fields, 19),
                            HasElevator = GetBoolValue(fields, 20),
                            HasVideoIntercom = GetBoolValue(fields, 21),
                            HasCooler = GetBoolValue(fields, 22),
                            HasRemoteControlDoor = GetBoolValue(fields, 23),
                            HasRadiator = GetBoolValue(fields, 24),
                            HasPackageHeater = GetBoolValue(fields, 25),
                            IsRenovated = GetBoolValue(fields, 26),
                            HasJacuzzi = GetBoolValue(fields, 27),
                            HasSauna = GetBoolValue(fields, 28),
                            HasPool = GetBoolValue(fields, 29),
                            HasLobby = GetBoolValue(fields, 30),
                            HasDuctSplit = GetBoolValue(fields, 31),
                            HasChiller = GetBoolValue(fields, 32),
                            HasRoofGarden = GetBoolValue(fields, 33),
                            HasMasterRoom = GetBoolValue(fields, 34),
                            HasNoElevator = GetBoolValue(fields, 35),
                            HasSplitAC = GetBoolValue(fields, 36),
                            HasJanitorService = GetBoolValue(fields, 37),
                            HasMeetingHall = GetBoolValue(fields, 38),
                            HasFanCoil = GetBoolValue(fields, 39),
                            HasGym = GetBoolValue(fields, 40),
                            HasCCTV = GetBoolValue(fields, 41),
                            HasEmergencyPower = GetBoolValue(fields, 42),
                            IsFlat = GetBoolValue(fields, 43),
                            HasUnderfloorHeating = GetBoolValue(fields, 44),
                            HasFireAlarm = GetBoolValue(fields, 45),
                            HasFireExtinguishingSystem = GetBoolValue(fields, 46),
                            HasCentralVacuum = GetBoolValue(fields, 47),
                            HasSecurityDoor = GetBoolValue(fields, 48),
                            HasLaundry = GetBoolValue(fields, 49),
                            IsFurnished = GetBoolValue(fields, 50),
                            IsSoldWithTenant = GetBoolValue(fields, 51),
                            HasCentralAntenna = GetBoolValue(fields, 52),
                            HasBackyard = GetBoolValue(fields, 53),
                            HasServantService = GetBoolValue(fields, 54),
                            HasBarbecue = GetBoolValue(fields, 55),
                            HasElectricalPanel = GetBoolValue(fields, 56),
                            HasConferenceHall = GetBoolValue(fields, 57),
                            HasGuardService = GetBoolValue(fields, 58),
                            HasAirHandler = GetBoolValue(fields, 59),
                            HasCentralSatellite = GetBoolValue(fields, 60),
                            HasGarbageChute = GetBoolValue(fields, 61),
                            HasLobbyManService = GetBoolValue(fields, 62),
                            HasGuardOrJanitorService = GetBoolValue(fields, 63)
                        };

                        realEstateList.Add(realEstate);
                        Console.WriteLine($"Successfully processed row {i + 1}: {realEstate.Mantaghe}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing row {i + 1}: {ex.Message}");
                        Console.WriteLine($"Row content: {dataLines[i]}");
                        continue;
                    }
                }

                CancellationToken cancellationToken = new CancellationToken();
                if (realEstateList.Any())
                {
                    Console.WriteLine($"Adding {realEstateList.Count} records to database");
                    _realEstateRepository.AddRangeAsync(realEstateList, cancellationToken).GetAwaiter().GetResult();
                    Console.WriteLine("Data initialization completed successfully");
                }
                else
                {
                    Console.WriteLine("No valid records found to add to database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in data initialization: {ex.Message}");
                throw;
            }
        }

        // متدهای کمکی برای پارس کردن داده‌ها
        private string[] ParseCsvLine(string line)
        {
            // پاک کردن carriage return و newline
            line = line.Replace("\r", "").Replace("\n", "");

            // جدا کردن فیلدها با در نظر گرفتن , و Tab
            var fields = Regex.Split(line, @"[\t,]");

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i]?.Trim() ?? string.Empty;

                // حذف کوتیشن های اضافی اگر وجود داشته باشد
                if (fields[i].StartsWith("\"") && fields[i].EndsWith("\"") && fields[i].Length > 1)
                {
                    fields[i] = fields[i].Substring(1, fields[i].Length - 2);
                }
            }

            Console.WriteLine($"Parsed {fields.Length} fields from line");

            return fields;
        }

        private string GetStringValue(string[] fields, int index)
        {
            if (index >= fields.Length) return string.Empty;
            var value = fields[index]?.Trim();

            // حذف کوتیشن های اضافی
            if (!string.IsNullOrEmpty(value) && value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        private int GetIntValue(string[] fields, int index)
        {
            if (index >= fields.Length) return 0;
            var value = GetStringValue(fields, index);
            if (string.IsNullOrEmpty(value)) return 0;

            if (int.TryParse(value, out int result))
                return result;

            Console.WriteLine($"Failed to parse int value: '{value}' at index {index}");
            return 0;
        }

        private decimal GetDecimalValue(string[] fields, int index)
        {
            if (index >= fields.Length) return 0;
            var value = GetStringValue(fields, index);
            if (string.IsNullOrEmpty(value)) return 0;

            // پردازش نوتیشن علمی مثل 7.15E+13
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                return result;

            // اگر به عنوان double پارس شد، به decimal تبدیل کن
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleResult))
            {
                try
                {
                    return Convert.ToDecimal(doubleResult);
                }
                catch
                {
                    Console.WriteLine($"Failed to convert double to decimal: '{value}' at index {index}");
                    return 0;
                }
            }

            Console.WriteLine($"Failed to parse decimal value: '{value}' at index {index}");
            return 0;
        }

        private bool GetBoolValue(string[] fields, int index)
        {
            if (index >= fields.Length) return false;
            var value = GetStringValue(fields, index);
            if (string.IsNullOrEmpty(value)) return false;

            return value == "1" || value.ToLower() == "true";
        }

        // پارس کردن نوع آشپزخانه
        private KitchenType ParseKitchenType(string value)
        {
            if (string.IsNullOrEmpty(value)) return KitchenType.Unknown;

            return value.ToLower().Trim() switch
            {
                "mdf" => KitchenType.MDF,
                "membrane" => KitchenType.Membrane,
                "furnished" => KitchenType.Furnished,
                "high-class" => KitchenType.HighClass,
                "custom" => KitchenType.Custom,
                "neo-classic" => KitchenType.NeoClassic,
                "wooden" => KitchenType.Wooden,
                "classic" => KitchenType.Classic,
                "german-pvc" => KitchenType.GermanPVC,
                "polished" => KitchenType.Polished,
                "metallic" => KitchenType.Metallic,
                "wood-metal" => KitchenType.WoodMetal,
                "modern" => KitchenType.Modern,
                "pantry" => KitchenType.Pantry,
                _ => KitchenType.Unknown
            };
        }

        // پارس کردن نوع سرویس بهداشتی
        private BathroomType ParseBathroomType(string value)
        {
            if (string.IsNullOrEmpty(value)) return BathroomType.Persian;

            return value.ToLower().Trim() switch
            {
                "western" => BathroomType.Western,
                "persian" => BathroomType.Persian,
                "persian-western" => BathroomType.PersianWestern,
                "public" => BathroomType.Public,
                "private" => BathroomType.Private,
                _ => BathroomType.Persian
            };
        }

        // پارس کردن نوع کف‌پوش
        private FlooringType ParseFlooringType(string value)
        {
            if (string.IsNullOrEmpty(value)) return FlooringType.Unknown;

            return value.ToLower().Trim() switch
            {
                "stone" => FlooringType.Stone,
                "ceramic" => FlooringType.Ceramic,
                "parquet" => FlooringType.Parquet,
                "laminate" => FlooringType.Laminate,
                "ceramic parquet" => FlooringType.CeramicParquet,
                "stone parquet" => FlooringType.StoneParquet,
                "carpet" => FlooringType.Carpet,
                "mosaic" => FlooringType.Mosaic,
                "mosaic stone" => FlooringType.MosaicStone,
                "wooden" => FlooringType.Wooden,
                "pvc" => FlooringType.PVC,
                _ => FlooringType.Unknown
            };
        }

        // پارس کردن نوع نما
        private FacadeType ParseFacadeType(string value)
        {
            if (string.IsNullOrEmpty(value)) return FacadeType.Unknown;

            return value.ToLower().Trim() switch
            {
                "stone" => FacadeType.Stone,
                "modern stone" => FacadeType.ModernStone,
                "roman stone" => FacadeType.RomanStone,
                "roman" => FacadeType.Roman,
                "classic" => FacadeType.Classic,
                "brick" => FacadeType.Brick,
                "3cm brick" => FacadeType.ThreeCmBrick,
                "stone & brick" => FacadeType.StoneAndBrick,
                "stone & glass" => FacadeType.StoneAndGlass,
                "cement" => FacadeType.Cement,
                "cement stone" => FacadeType.CementStone,
                "glass" => FacadeType.Glass,
                "ceramic" => FacadeType.Ceramic,
                "thermo wood" => FacadeType.ThermoWood,
                "granite" => FacadeType.Granite,
                "composite" => FacadeType.Composite,
                "composite panel" => FacadeType.CompositePanel,
                _ => FacadeType.Unknown
            };
        }

        private DocumentType ParseDocumentType(string value)
        {
            if (string.IsNullOrEmpty(value)) return DocumentType.Unknown;

            return value.ToLower().Trim() switch
            {
                "personal" => DocumentType.Personal,
                "residential" => DocumentType.Residential,
                "endowment" => DocumentType.Endowment,
                "promissory" => DocumentType.Promissory,
                "administrative" => DocumentType.Administrative,
                "commercial" => DocumentType.Commercial,
                "guidance endowment" => DocumentType.GuidanceEndowment,
                "cooperative" => DocumentType.Cooperative,
                _ => DocumentType.Unknown
            };
        }
    }
}