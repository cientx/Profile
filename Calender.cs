using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class USER_FEST_INFO
{
	public uint type_festival = 0;
	public uint id_festival = 0;
	public string name = string.Empty;
	public int year_defined = 0;
	public Date date_main = new Date();
	public Date date_sub = new Date();
};
public class TUserFestivals : List<USER_FEST_INFO> { }
public class TSysFestivals : List<MdsSysFest> { }

class Calender
{
	//-----------------------------------  음력 및 간지 계산 함수들  --------------------------------------------------------
	const int year_org = 1881;
	const int min_day = 687052;	// 1881.1.30 (lunar 1881.1.1)
	const int max_day = 749154;	// 2051.2.10 (lunar 2050.12.29)

	public static int gc_yearMin				= 1900;
	public static int gc_yearMax				= 2050;

	const int days_min				= 693962;	// 到公历1900年1月1日的总天数
	const int year_min_gan			= 6;		// 庚
	const int year_min_zi			= 0;		// 子
	const int year_min_nick			= 0;		// 1900年是鼠年

	const int month_min_gan			= 4;		// 戊  农历1900年1月是戊寅月
	const int month_min_zi			= 2;		// 寅
	const int day_min_gan			= 0;		// 甲  公历1900年1月1日是甲戌日
	const int day_min_zi			= 10;		// 戌

	// Index of table_days[] represents escaped_lunar_months from (lunar) 1881.1.1(including) to table_days[escaped_lunar_months].
	public static int[] table_days = new int[] {
			0x0000,
			0x001D, 0x003B, 0x0058, 0x0076, 0x0093, 0x00B1, 0x00CF, 0x00EC, 0x010A, 0x0128, 0x0145, 0x0163, 0x0180,	// 1881/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0x019D, 0x01BB, 0x01D8, 0x01F6, 0x0213, 0x0231, 0x024E, 0x026C, 0x028A, 0x02A7,	0x02C5, 0x02E3,			// 1882
			0x0300, 0x031D, 0x033B, 0x0358, 0x0375, 0x0393, 0x03B0, 0x03CE, 0x03EC, 0x040A, 0x0427,	0x0445,			// 1883
			0x0463, 0x0480, 0x049D, 0x04BB, 0x04D8, 0x04F5, 0x0513, 0x0530, 0x054E, 0x056C, 0x0589, 0x05A7,	0x05C5,	// 1884/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x05E3, 0x0600, 0x061D, 0x063B, 0x0658, 0x0675, 0x0693, 0x06B0, 0x06CE, 0x06EB, 0x0709, 0x0727,			// 1885
			0x0745, 0x0762, 0x0780, 0x079D, 0x07BB, 0x07D8, 0x07F5, 0x0813, 0x0830, 0x084E, 0x086B, 0x0889,			// 1886
			0x08A7,	0x08C5, 0x08E2, 0x0900, 0x091D, 0x093B, 0x0958, 0x0975, 0x0993, 0x09B0, 0x09CE, 0x09EB, 0x0A09,	// 1887/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x0A27,	0x0A44, 0x0A62, 0x0A80, 0x0A9D, 0x0ABB, 0x0AD8, 0x0AF5, 0x0B13, 0x0B30, 0x0B4E, 0x0B6B,			// 1888
			0x0B89, 0x0BA6,	0x0BC4, 0x0BE2, 0x0BFF, 0x0C1D, 0x0C3A, 0x0C58, 0x0C75, 0x0C93, 0x0CB0, 0x0CCE,			// 1889
			0x0CEB, 0x0D09, 0x0D26,	0x0D44, 0x0D61, 0x0D7F, 0x0D9D, 0x0DBA, 0x0DD8, 0x0DF5, 0x0E13, 0x0E30, 0x0E4E,	// 1890/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x0E6B, 0x0E89, 0x0EA6,	0x0EC4, 0x0EE1, 0x0EFF, 0x0F1C, 0x0F3A, 0x0F58, 0x0F75, 0x0F93, 0x0FB1,			// 1891
			0x0FCE, 0x0FEB, 0x1009, 0x1026,	0x1043, 0x1061, 0x107E, 0x109C, 0x10BA, 0x10D7, 0x10F5, 0x1113, 0x1131,	// 1892/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x114E, 0x116B, 0x1189, 0x11A6,	0x11C3, 0x11E1, 0x11FE, 0x121C, 0x1239, 0x1257, 0x1275, 0x1293,			// 1893
			0x12B0, 0x12CE, 0x12EB, 0x1309, 0x1326,	0x1343, 0x1361, 0x137E, 0x139C, 0x13B9, 0x13D7, 0x13F5,			// 1894
			0x1413, 0x1430, 0x144E, 0x146B, 0x1489, 0x14A6,	0x14C3, 0x14E1, 0x14FE, 0x151C, 0x1539, 0x1557, 0x1574,	// 1895/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x1592, 0x15B0, 0x15CE, 0x15EB, 0x1609, 0x1626,	0x1643, 0x1661, 0x167E, 0x169C, 0x16B9, 0x16D7,			// 1896
			0x16F4, 0x1712, 0x1730, 0x174D, 0x176B, 0x1788, 0x17A6,	0x17C3, 0x17E1, 0x17FE, 0x181C, 0x1839,			// 1897
			0x1857, 0x1874, 0x1892, 0x18AF, 0x18CD, 0x18EB, 0x1908, 0x1926,	0x1943, 0x1961, 0x197E, 0x199C, 0x19B9,	// 1898/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x19D7, 0x19F4, 0x1A12, 0x1A2F, 0x1A4D, 0x1A6A, 0x1A88, 0x1AA6,	0x1AC3, 0x1AE1, 0x1AFE, 0x1B1C,			// 1899
			0x1B39, 0x1B57, 0x1B74, 0x1B91, 0x1BAF, 0x1BCC, 0x1BEA, 0x1C08, 0x1C25,	0x1C43, 0x1C61, 0x1C7E, 0x1C9C,	// 1900/1, 2, 3, 4, 5, 6, 7, 8, run8, 9, 10, 11, 12
			0x1CB9, 0x1CD7, 0x1CF4, 0x1D11, 0x1D2F, 0x1D4C, 0x1D6A, 0x1D87, 0x1DA5,	0x1DC3, 0x1DE1, 0x1DFE,			// 1901
			0x1E1C, 0x1E39, 0x1E57, 0x1E74, 0x1E91, 0x1EAF, 0x1ECC, 0x1EEA, 0x1F07, 0x1F25,	0x1F43, 0x1F61,			// 1902
			0x1F7E, 0x1F9C, 0x1FB9, 0x1FD7, 0x1FF4, 0x2011, 0x202F, 0x204C, 0x2069, 0x2087, 0x20A5,	0x20C2, 0x20E0,	// 1903/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x20FE, 0x211C, 0x2139, 0x2157, 0x2174, 0x2191, 0x21AF, 0x21CC, 0x21E9, 0x2207, 0x2225,	0x2242,			// 1904
			0x2260, 0x227E, 0x229B, 0x22B9, 0x22D7, 0x22F4, 0x2311, 0x232F, 0x234C, 0x236A, 0x2387, 0x23A5,			// 1905
			0x23C2, 0x23E0, 0x23FE, 0x241B, 0x2439, 0x2456, 0x2474, 0x2491, 0x24AF, 0x24CC, 0x24EA, 0x2507, 0x2525,	// 1906/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x2542, 0x2560, 0x257D, 0x259B, 0x25B8, 0x25D6, 0x25F4, 0x2611, 0x262F, 0x264C, 0x266A, 0x2687,			// 1907
			0x26A5,	0x26C2, 0x26DF, 0x26FD, 0x271B, 0x2738, 0x2756, 0x2773, 0x2791, 0x27AF, 0x27CC, 0x27EA,			// 1908
			0x2807, 0x2825,	0x2842, 0x285F, 0x287D, 0x289A, 0x28B8, 0x28D5, 0x28F3, 0x2911, 0x292F, 0x294C, 0x296A,	// 1909/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x2987, 0x29A5,	0x29C2, 0x29DF, 0x29FD, 0x2A1A, 0x2A38, 0x2A55, 0x2A73, 0x2A91, 0x2AAF, 0x2ACC,			// 1910
			0x2AEA, 0x2B07, 0x2B25,	0x2B42, 0x2B5F, 0x2B7D, 0x2B9A, 0x2BB7, 0x2BD5, 0x2BF3, 0x2C10, 0x2C2E, 0x2C4C,	// 1911/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x2C6A, 0x2C87, 0x2CA5,	0x2CC2, 0x2CDF, 0x2CFD, 0x2D1A, 0x2D37, 0x2D55, 0x2D73, 0x2D90, 0x2DAE,			// 1912
			0x2DCC, 0x2DEA, 0x2E07, 0x2E25,	0x2E42, 0x2E5F, 0x2E7D, 0x2E9A, 0x2EB7, 0x2ED5, 0x2EF2, 0x2F10,			// 1913
			0x2F2E, 0x2F4C, 0x2F69, 0x2F87, 0x2FA5,	0x2FC2, 0x2FDF, 0x2FFD, 0x301A, 0x3038, 0x3055, 0x3072, 0x3090,	// 1914/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x30AE, 0x30CB, 0x30E9, 0x3107, 0x3124,	0x3142, 0x315F, 0x317D, 0x319A, 0x31B8, 0x31D5, 0x31F3,			// 1915
			0x3210, 0x322E, 0x324B, 0x3269, 0x3286, 0x32A4,	0x32C2, 0x32DF, 0x32FD, 0x331A, 0x3338, 0x3355,			// 1916
			0x3373, 0x3390, 0x33AD, 0x33CB, 0x33E8, 0x3406, 0x3424,	0x3441, 0x345F, 0x347D, 0x349A, 0x34B8, 0x34D5,	// 1917/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x34F3, 0x3510, 0x352D, 0x354B, 0x3568, 0x3586, 0x35A3,	0x35C1, 0x35DF, 0x35FD, 0x361A, 0x3638,			// 1918
			0x3655, 0x3673, 0x3690, 0x36AD, 0x36CB, 0x36E8, 0x3706, 0x3723,	0x3741, 0x375F, 0x377C, 0x379A, 0x37B8,	// 1919/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0x37D5, 0x37F3, 0x3810, 0x382D, 0x384B, 0x3868, 0x3885, 0x38A3,	0x38C1, 0x38DE, 0x38FC, 0x391A,			// 1920
			0x3938, 0x3955, 0x3973, 0x3990, 0x39AD, 0x39CB, 0x39E8, 0x3A05, 0x3A23,	0x3A40, 0x3A5E, 0x3A7C,			// 1921
			0x3A9A, 0x3AB7, 0x3AD5, 0x3AF3, 0x3B10, 0x3B2D, 0x3B4B, 0x3B68, 0x3B85, 0x3BA3,	0x3BC0, 0x3BDE, 0x3BFC,	// 1922/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x3C19, 0x3C37, 0x3C55, 0x3C72, 0x3C90, 0x3CAD, 0x3CCB, 0x3CE8, 0x3D06, 0x3D23,	0x3D40, 0x3D5E,			// 1923
			0x3D7C, 0x3D99, 0x3DB7, 0x3DD4, 0x3DF2, 0x3E10, 0x3E2D, 0x3E4B, 0x3E68, 0x3E86, 0x3EA3,	0x3EC0,			// 1924
			0x3EDE, 0x3EFB, 0x3F19, 0x3F37, 0x3F54, 0x3F72, 0x3F8F, 0x3FAD, 0x3FCB, 0x3FE8, 0x4006, 0x4023,	0x4041,	// 1925/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x405E, 0x407B, 0x4099, 0x40B6, 0x40D4, 0x40F1, 0x410F, 0x412D, 0x414A, 0x4168, 0x4186, 0x41A3,			// 1926
			0x41C1, 0x41DE, 0x41FB, 0x4219, 0x4236, 0x4254, 0x4271, 0x428F, 0x42AD, 0x42CA, 0x42E8, 0x4306,			// 1927
			0x4323,	0x4341, 0x435E, 0x437B, 0x4399, 0x43B6, 0x43D3, 0x43F1, 0x440F, 0x442C, 0x444A, 0x4468, 0x4486,	// 1928/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x44A3,	0x44C1, 0x44DE, 0x44FB, 0x4519, 0x4536, 0x4553, 0x4571, 0x458E, 0x45AC, 0x45CA, 0x45E8,			// 1929
			0x4605, 0x4623,	0x4641, 0x465E, 0x467B, 0x4699, 0x46B6, 0x46D3, 0x46F1, 0x470E, 0x472C, 0x474A, 0x4767,	// 1930/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x4785, 0x47A3,	0x47C1, 0x47DE, 0x47FB, 0x4819, 0x4836, 0x4853, 0x4871, 0x488E, 0x48AC, 0x48C9,			// 1931
			0x48E7, 0x4905, 0x4923,	0x4940, 0x495E, 0x497B, 0x4999, 0x49B6, 0x49D3, 0x49F1, 0x4A0E, 0x4A2C,			// 1932
			0x4A49, 0x4A67, 0x4A85, 0x4AA2,	0x4AC0, 0x4ADE, 0x4AFB, 0x4B19, 0x4B36, 0x4B54, 0x4B71, 0x4B8E, 0x4BAC,	// 1933/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x4BC9, 0x4BE7, 0x4C04, 0x4C22,	0x4C40, 0x4C5D, 0x4C7B, 0x4C99, 0x4CB6, 0x4CD4, 0x4CF1, 0x4D0F,			// 1934
			0x4D2C, 0x4D49, 0x4D67, 0x4D84, 0x4DA2,	0x4DBF, 0x4DDD, 0x4DFB, 0x4E18, 0x4E36, 0x4E54, 0x4E71,			// 1935
			0x4E8F, 0x4EAC, 0x4EC9, 0x4EE7, 0x4F04, 0x4F22,	0x4F3F, 0x4F5D, 0x4F7A, 0x4F98, 0x4FB6, 0x4FD4, 0x4FF1,	// 1936/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x500F, 0x502C, 0x5049, 0x5067, 0x5084, 0x50A1,	0x50BF, 0x50DC, 0x50FA, 0x5118, 0x5136, 0x5153,			// 1937
			0x5171, 0x518F, 0x51AC, 0x51C9, 0x51E7, 0x5204, 0x5221,	0x523F, 0x525C, 0x527A, 0x5298, 0x52B5, 0x52D3,	// 1938/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0x52F1, 0x530F, 0x532C, 0x5349, 0x5367, 0x5384, 0x53A1,	0x53BF, 0x53DC, 0x53FA, 0x5417, 0x5435,			// 1939
			0x5453, 0x5471, 0x548E, 0x54AC, 0x54C9, 0x54E7, 0x5504, 0x5521,	0x553F, 0x555C, 0x557A, 0x5597,			// 1940
			0x55B5, 0x55D3, 0x55F0, 0x560E, 0x562C, 0x5649, 0x5667, 0x5684, 0x56A1,	0x56BF, 0x56DC, 0x56FA, 0x5717,	// 1941/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x5735, 0x5752, 0x5770, 0x578E, 0x57AB, 0x57C9, 0x57E7, 0x5804, 0x5822,	0x583F, 0x585C, 0x587A,			// 1942
			0x5897, 0x58B5, 0x58D2, 0x58F0, 0x590D, 0x592B, 0x5949, 0x5966, 0x5984, 0x59A2,	0x59BF, 0x59DD,			// 1943
			0x59FA, 0x5A17, 0x5A35, 0x5A52, 0x5A70, 0x5A8D, 0x5AAB, 0x5AC8, 0x5AE6, 0x5B04, 0x5B21,	0x5B3F, 0x5B5D,	// 1944/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x5B7A, 0x5B97, 0x5BB5, 0x5BD2, 0x5BEF, 0x5C0D, 0x5C2A, 0x5C48, 0x5C66, 0x5C84, 0x5CA1,	0x5CBF,			// 1945
			0x5CDD, 0x5CFA, 0x5D17, 0x5D35, 0x5D52, 0x5D6F, 0x5D8D, 0x5DAA, 0x5DC8, 0x5DE6, 0x5E03, 0x5E21,			// 1946
			0x5E3F, 0x5E5D, 0x5E7A, 0x5E97, 0x5EB5, 0x5ED2, 0x5EEF, 0x5F0D, 0x5F2A, 0x5F48, 0x5F65, 0x5F83, 0x5FA1,	// 1947/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x5FBF, 0x5FDC, 0x5FFA, 0x6017, 0x6035, 0x6052, 0x606F, 0x608D, 0x60AA, 0x60C8, 0x60E5, 0x6103,			// 1948
			0x6121,	0x613F, 0x615C, 0x617A, 0x6197, 0x61B5, 0x61D2, 0x61EF, 0x620D, 0x622A, 0x6248, 0x6265, 0x6283,	// 1949/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0x62A1,	0x62BE, 0x62DC, 0x62FA, 0x6317, 0x6335, 0x6352, 0x636F, 0x638D, 0x63AA, 0x63C8, 0x63E5,			// 1950
			0x6403, 0x6420,	0x643E, 0x645C, 0x6479, 0x6497, 0x64B4, 0x64D2, 0x64EF, 0x650D, 0x652A, 0x6548,			// 1951
			0x6565, 0x6583, 0x65A0,	0x65BE, 0x65DB, 0x65F9, 0x6617, 0x6634, 0x6652, 0x666F, 0x668D, 0x66AA, 0x66C8,	// 1952/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x66E5, 0x6703, 0x6720,	0x673D, 0x675B, 0x6779, 0x6796, 0x67B4, 0x67D2, 0x67EF, 0x680D, 0x682B,			// 1953
			0x6848, 0x6865, 0x6883, 0x68A0,	0x68BD, 0x68DB, 0x68F8, 0x6916, 0x6934, 0x6951, 0x696F, 0x698D,			// 1954
			0x69AB, 0x69C8, 0x69E5, 0x6A03, 0x6A20,	0x6A3D, 0x6A5B, 0x6A78, 0x6A96, 0x6AB3, 0x6AD1, 0x6AEF, 0x6B0D,	// 1955/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x6B2A, 0x6B48, 0x6B65, 0x6B83, 0x6BA0,	0x6BBD, 0x6BDB, 0x6BF8, 0x6C16, 0x6C33, 0x6C51, 0x6C6F,			// 1956
			0x6C8D, 0x6CAA, 0x6CC8, 0x6CE5, 0x6D03, 0x6D20,	0x6D3D, 0x6D5B, 0x6D78, 0x6D96, 0x6DB3, 0x6DD1, 0x6DEF,	// 1957/1, 2, 3, 4, 5, 6, 7, 8, 9, 10, run10, 11, 12
			0x6E0C, 0x6E2A, 0x6E48, 0x6E65, 0x6E83, 0x6EA0,	0x6EBD, 0x6EDB, 0x6EF8, 0x6F16, 0x6F33, 0x6F51,			// 1958
			0x6F6E, 0x6F8C, 0x6FAA, 0x6FC7, 0x6FE5, 0x7002, 0x7020,	0x703D, 0x705B, 0x7078, 0x7096, 0x70B3,			// 1959
			0x70D1, 0x70EE, 0x710C, 0x7129, 0x7147, 0x7165, 0x7182, 0x71A0,	0x71BD, 0x71DB, 0x71F8, 0x7216, 0x7233,	// 1960/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x7251, 0x726E, 0x728C, 0x72A9, 0x72C7, 0x72E4, 0x7302, 0x7320,	0x733D, 0x735B, 0x7378, 0x7396,			// 1961
			0x73B3, 0x73D1, 0x73EE, 0x740B, 0x7429, 0x7446, 0x7464, 0x7482, 0x749F,	0x74BD, 0x74DB, 0x74F8,			// 1962
			0x7516, 0x7533, 0x7551, 0x756E, 0x758B, 0x75A9, 0x75C6, 0x75E4, 0x7601, 0x761F,	0x763D, 0x765B, 0x7678,	// 1963/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x7696, 0x76B3, 0x76D1, 0x76EE, 0x770B, 0x7729, 0x7746, 0x7764, 0x7781, 0x779F,	0x77BD, 0x77DB,			// 1964
			0x77F8, 0x7816, 0x7833, 0x7851, 0x786E, 0x788B, 0x78A9, 0x78C6, 0x78E3, 0x7901, 0x791F,	0x793D,			// 1965
			0x795A, 0x7978, 0x7996, 0x79B3, 0x79D1, 0x79EE, 0x7A0B, 0x7A29, 0x7A46, 0x7A63, 0x7A81, 0x7A9F,	0x7ABC,	// 1966/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0x7ADA, 0x7AF8, 0x7B15, 0x7B33, 0x7B51, 0x7B6E, 0x7B8B, 0x7BA9, 0x7BC6, 0x7BE4, 0x7C01, 0x7C1F,			// 1967
			0x7C3C, 0x7C5A, 0x7C78, 0x7C95, 0x7CB3, 0x7CD0, 0x7CEE, 0x7D0B, 0x7D29, 0x7D46, 0x7D64, 0x7D81, 0x7D9F,	// 1968/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0x7DBC, 0x7DDA, 0x7DF7, 0x7E15, 0x7E32, 0x7E50, 0x7E6E, 0x7E8B, 0x7EA9, 0x7EC6, 0x7EE4, 0x7F01,			// 1969
			0x7F1F,	0x7F3C, 0x7F59, 0x7F77, 0x7F95, 0x7FB2, 0x7FD0, 0x7FED, 0x800B, 0x8029, 0x8046, 0x8064,			// 1970
			0x8081, 0x809F,	0x80BC, 0x80D9, 0x80F7, 0x8114, 0x8132, 0x814F, 0x816D, 0x818B, 0x81A9, 0x81C6, 0x81E4,	// 1971/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x8201, 0x821F,	0x823C, 0x8259, 0x8277, 0x8294, 0x82B2, 0x82CF, 0x82ED, 0x830B, 0x8329, 0x8346,			// 1972
			0x8364, 0x8381, 0x839F,	0x83BC, 0x83D9, 0x83F7, 0x8414, 0x8431, 0x844F, 0x846D, 0x848B, 0x84A8,			// 1973
			0x84C6, 0x84E4, 0x8501, 0x851F,	0x853C, 0x8559, 0x8577, 0x8594, 0x85B1, 0x85CF, 0x85ED, 0x860A, 0x8628,	// 1974/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x8646, 0x8664, 0x8681, 0x869F,	0x86BC, 0x86D9, 0x86F7, 0x8714, 0x8731, 0x874F, 0x876C, 0x878A,			// 1975
			0x87A8, 0x87C6, 0x87E3, 0x8801, 0x881E,	0x883C, 0x8859, 0x8877, 0x8894, 0x88B2, 0x88CF, 0x88EC, 0x890A,	// 1976/1, 2, 3, 4, 5, 6, 7, 8, 9, 10, run10, 11, 12
			0x8928, 0x8945, 0x8963, 0x8981, 0x899E,	0x89BC, 0x89D9, 0x89F7, 0x8A14, 0x8A32, 0x8A4F, 0x8A6C,			// 1977
			0x8A8A, 0x8AA8, 0x8AC5, 0x8AE3, 0x8B00, 0x8B1E,	0x8B3C, 0x8B59, 0x8B77, 0x8B94, 0x8BB2, 0x8BCF,			// 1978
			0x8BED, 0x8C0A, 0x8C27, 0x8C45, 0x8C62, 0x8C80, 0x8C9E,	0x8CBB, 0x8CD9, 0x8CF7, 0x8D14, 0x8D32, 0x8D4F,	// 1979/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x8D6D, 0x8D8A, 0x8DA7, 0x8DC5, 0x8DE2, 0x8E00, 0x8E1D,	0x8E3B, 0x8E59, 0x8E76, 0x8E94, 0x8EB2,			// 1980
			0x8ECF, 0x8EED, 0x8F0A, 0x8F27, 0x8F45, 0x8F62, 0x8F7F, 0x8F9D,	0x8FBB, 0x8FD8, 0x8FF6, 0x9014,			// 1981
			0x9032, 0x904F, 0x906D, 0x908A, 0x90A7, 0x90C5, 0x90E2, 0x90FF, 0x911D,	0x913B, 0x9158, 0x9176, 0x9194,	// 1982/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0x91B2, 0x91CF, 0x91ED, 0x920A, 0x9227, 0x9245, 0x9262, 0x927F, 0x929D,	0x92BA, 0x92D8, 0x92F6,			// 1983
			0x9314, 0x9331, 0x934F, 0x936D, 0x938A, 0x93A7, 0x93C5, 0x93E2, 0x93FF, 0x941D,	0x943A, 0x9458, 0x9476,	// 1984/1, 2, 3, 4, 5, 6, 7, 8, 9, 10, run10, 11, 12
			0x9493, 0x94B1, 0x94CF, 0x94EC, 0x950A, 0x9527, 0x9545, 0x9562, 0x957F, 0x959D,	0x95BA, 0x95D8,			// 1985
			0x95F5, 0x9613, 0x9631, 0x964E, 0x966C, 0x968A, 0x96A7, 0x96C5, 0x96E2, 0x9700, 0x971D,	0x973A,			// 1986
			0x9758, 0x9775, 0x9793, 0x97B1, 0x97CE, 0x97EC, 0x9809, 0x9827, 0x9845, 0x9862, 0x9880, 0x989D,	0x98BB,	// 1987/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0x98D8, 0x98F5, 0x9913, 0x9930, 0x994E, 0x996B, 0x9989, 0x99A7, 0x99C4, 0x99E2, 0x9A00, 0x9A1D,			// 1988
			0x9A3B, 0x9A58, 0x9A75, 0x9A93, 0x9AB0, 0x9ACE, 0x9AEB, 0x9B09, 0x9B27, 0x9B44, 0x9B62, 0x9B80,			// 1989
			0x9B9D,	0x9BBB, 0x9BD8, 0x9BF5, 0x9C13, 0x9C30, 0x9C4D, 0x9C6B, 0x9C89, 0x9CA6, 0x9CC4, 0x9CE2, 0x9D00,	// 1990/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0x9D1D,	0x9D3B, 0x9D58, 0x9D75, 0x9D93, 0x9DB0, 0x9DCD, 0x9DEB, 0x9E08, 0x9E26, 0x9E44, 0x9E62,			// 1991
			0x9E7F, 0x9E9D,	0x9EBB, 0x9ED8, 0x9EF5, 0x9F13, 0x9F30, 0x9F4D, 0x9F6B, 0x9F88, 0x9FA6, 0x9FC4,			// 1992
			0x9FE1, 0x9FFF, 0xA01D,	0xA03A, 0xA058, 0xA075, 0xA093, 0xA0B0, 0xA0CD, 0xA0EB, 0xA108, 0xA126, 0xA143,	// 1993/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0xA161, 0xA17F, 0xA19D,	0xA1BA, 0xA1D8, 0xA1F5, 0xA213, 0xA230, 0xA24D, 0xA26B, 0xA288, 0xA2A6,			// 1994
			0xA2C3, 0xA2E1, 0xA2FF, 0xA31C,	0xA33A, 0xA358, 0xA375, 0xA393, 0xA3B0, 0xA3CE, 0xA3EB, 0xA408, 0xA426,	// 1995/1, 2, 3, 4, 5, 6, 7, 8, 9, 10, run10, 11, 12
			0xA443, 0xA461, 0xA47E, 0xA49C,	0xA4BA, 0xA4D7, 0xA4F5, 0xA512, 0xA530, 0xA54E, 0xA56B, 0xA589,			// 1996
			0xA5A6, 0xA5C3, 0xA5E1, 0xA5FE, 0xA61C,	0xA639, 0xA657, 0xA675, 0xA692, 0xA6B0, 0xA6CE, 0xA6EB,			// 1997
			0xA709, 0xA726, 0xA743, 0xA761, 0xA77E, 0xA79B,	0xA7B9, 0xA7D7, 0xA7F4, 0xA812, 0xA830, 0xA84E, 0xA86B,	// 1998/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0xA889, 0xA8A6, 0xA8C3, 0xA8E1, 0xA8FE, 0xA91B,	0xA939, 0xA956, 0xA974, 0xA992, 0xA9B0, 0xA9CD,			// 1999
			0xA9EB, 0xAA09, 0xAA26, 0xAA43, 0xAA61, 0xAA7E, 0xAA9B,	0xAAB9, 0xAAD6, 0xAAF4, 0xAB12, 0xAB2F,			// 2000
			0xAB4D, 0xAB6B, 0xAB89, 0xABA6, 0xABC3, 0xABE1, 0xABFE, 0xAC1B,	0xAC39, 0xAC56, 0xAC74, 0xAC91, 0xACAF,	// 2001/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0xACCD, 0xACEB, 0xAD08, 0xAD26, 0xAD43, 0xAD61, 0xAD7E, 0xAD9B,	0xADB9, 0xADD6, 0xADF4, 0xAE11,			// 2002
			0xAE2F, 0xAE4D, 0xAE6A, 0xAE88, 0xAEA6, 0xAEC3, 0xAEE1, 0xAEFE, 0xAF1B,	0xAF39, 0xAF56, 0xAF74,			// 2003
			0xAF91, 0xAFAF, 0xAFCC, 0xAFEA, 0xB008, 0xB025, 0xB043, 0xB060, 0xB07E, 0xB09B,	0xB0B9, 0xB0D6, 0xB0F4,	// 2004/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0xB111, 0xB12F, 0xB14C, 0xB16A, 0xB187, 0xB1A5, 0xB1C3, 0xB1E0, 0xB1FE, 0xB21C,	0xB239, 0xB256,			// 2005
			0xB274, 0xB291, 0xB2AF, 0xB2CC, 0xB2EA, 0xB307, 0xB325, 0xB342, 0xB360, 0xB37E, 0xB39B,	0xB3B9, 0xB3D7,	// 2006/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0xB3F4, 0xB411, 0xB42F, 0xB44C, 0xB469, 0xB487, 0xB4A4, 0xB4C2, 0xB4E0, 0xB4FE, 0xB51B,	0xB539,			// 2007
			0xB557, 0xB574, 0xB591, 0xB5AF, 0xB5CC, 0xB5E9, 0xB607, 0xB624, 0xB642, 0xB660, 0xB67D, 0xB69B,			// 2008
			0xB6B9, 0xB6D7, 0xB6F4, 0xB711, 0xB72F, 0xB74C, 0xB769, 0xB787, 0xB7A4, 0xB7C2, 0xB7DF, 0xB7FD, 0xB81B,	// 2009/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0xB839, 0xB856, 0xB874, 0xB891, 0xB8AF, 0xB8CC, 0xB8E9, 0xB907, 0xB924, 0xB942, 0xB95F, 0xB97D,			// 2010
			0xB99B,	0xB9B8, 0xB9D6, 0xB9F4, 0xBA11, 0xBA2F, 0xBA4C, 0xBA69, 0xBA87, 0xBAA4, 0xBAC2, 0xBADF,			// 2011
			0xBAFD, 0xBB1A, 0xBB38, 0xBB56, 0xBB73, 0xBB91, 0xBBAE, 0xBBCC, 0xBBE9, 0xBC07, 0xBC24, 0xBC42, 0xBC5F,	// 2012/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0xBC7D, 0xBC9A,	0xBCB8, 0xBCD5, 0xBCF3, 0xBD11, 0xBD2E, 0xBD4C, 0xBD69, 0xBD87, 0xBDA4, 0xBDC2,			// 2013
			0xBDDF, 0xBDFD, 0xBE1A,	0xBE38, 0xBE55, 0xBE73, 0xBE90, 0xBEAE, 0xBECC, 0xBEE9, 0xBF07, 0xBF24, 0xBF42,	// 2014/1, 2, 3, 4, 5, 6, 7, 8, 9, run9, 10, 11, 12
			0xBF5F, 0xBF7D, 0xBF9A, 0xBFB7, 0xBFD5, 0xBFF2, 0xC010, 0xC02E, 0xC04C, 0xC069, 0xC087, 0xC0A4,			// 2015
			0xC0C2, 0xC0DF, 0xC0FD, 0xC11A,	0xC137, 0xC155, 0xC172, 0xC190, 0xC1AE, 0xC1CB, 0xC1E9, 0xC207,			// 2016
			0xC224, 0xC242, 0xC25F, 0xC27D, 0xC29A, 0xC2B7, 0xC2D5, 0xC2F2, 0xC310, 0xC32D, 0xC34B, 0xC369, 0xC387,	// 2017/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0xC3A4, 0xC3C2, 0xC3DF, 0xC3FD, 0xC41A, 0xC437, 0xC455, 0xC472, 0xC490, 0xC4AD, 0xC4CB, 0xC4E9,			// 2018
			0xC507, 0xC524, 0xC542, 0xC55F, 0xC57D, 0xC59A, 0xC5B7, 0xC5D5, 0xC5F2, 0xC60F, 0xC62D, 0xC64B,			// 2019
			0xC668, 0xC686, 0xC6A4, 0xC6C2, 0xC6DF, 0xC6FD, 0xC71A, 0xC737, 0xC755, 0xC772, 0xC790, 0xC7AD, 0xC7CB,	// 2020/1, 2, 3, 4, run4, 5, 6, 7, 8, 9, 10, 11, 12
			0xC7E8, 0xC806, 0xC824, 0xC841, 0xC85F, 0xC87C, 0xC89A, 0xC8B7, 0xC8D5, 0xC8F2, 0xC910, 0xC92D,			// 2021
			0xC94B, 0xC968, 0xC986, 0xC9A3, 0xC9C1, 0xC9DF, 0xC9FC, 0xCA1A, 0xCA37, 0xCA55, 0xCA72, 0xCA90,			// 2022
			0xCAAD, 0xCACB, 0xCAE8, 0xCB05, 0xCB23, 0xCB41, 0xCB5E, 0xCB7C, 0xCB9A, 0xCBB7, 0xCBD5, 0xCBF2, 0xCC10,	// 2023/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0xCC2D, 0xCC4B, 0xCC68, 0xCC85, 0xCCA3, 0xCCC0, 0xCCDE, 0xCCFC, 0xCD19,	0xCD37, 0xCD55, 0xCD72,			// 2024
			0xCD90, 0xCDAD, 0xCDCB, 0xCDE8, 0xCE05, 0xCE23, 0xCE40, 0xCE5E, 0xCE7B, 0xCE99,	0xCEB7, 0xCED5, 0xCEF2,	// 2025/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0xCF10, 0xCF2D, 0xCF4B, 0xCF68, 0xCF85, 0xCFA3, 0xCFC0, 0xCFDD, 0xCFFB, 0xD019,	0xD037, 0xD054,			// 2026
			0xD072, 0xD090, 0xD0AD, 0xD0CB, 0xD0E8, 0xD105, 0xD123, 0xD140, 0xD15D, 0xD17B, 0xD199,	0xD1B6,			// 2027
			0xD1D4, 0xD1F2, 0xD210, 0xD22D, 0xD24B, 0xD268, 0xD285, 0xD2A3, 0xD2C0, 0xD2DD, 0xD2FB, 0xD319, 0xD336,	// 2028/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0xD354, 0xD372, 0xD38F, 0xD3AD, 0xD3CA, 0xD3E8, 0xD405, 0xD423, 0xD440, 0xD45D, 0xD47B, 0xD499,			// 2029
			0xD4B6, 0xD4D4, 0xD4F1, 0xD50F, 0xD52D, 0xD54A, 0xD568, 0xD585, 0xD5A3, 0xD5C0, 0xD5DE, 0xD5FB,			// 2030
			0xD618,	0xD636, 0xD654, 0xD671, 0xD68F, 0xD6AC, 0xD6CA, 0xD6E8, 0xD705, 0xD723, 0xD740, 0xD75E, 0xD77B,	// 2031/1, 2, 3, run3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0xD799,	0xD7B6, 0xD7D3, 0xD7F1, 0xD80E, 0xD82C, 0xD84A, 0xD867, 0xD885, 0xD8A3, 0xD8C0, 0xD8DE,			// 2032
			0xD8FB, 0xD919,	0xD936, 0xD953, 0xD971, 0xD98E, 0xD9AC, 0xD9C9, 0xD9E7, 0xDA05, 0xDA23, 0xDA40, 0xDA5E,	// 2033/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0xDA7B, 0xDA99,	0xDAB6, 0xDAD3, 0xDAF1, 0xDB0E, 0xDB2C, 0xDB49, 0xDB67, 0xDB85, 0xDBA2, 0xDBC0,			// 2034
			0xDBDE, 0xDBFB, 0xDC19,	0xDC36, 0xDC53, 0xDC71, 0xDC8E, 0xDCAB, 0xDCC9, 0xDCE7, 0xDD04, 0xDD22,			// 2035
			0xDD40, 0xDD5E, 0xDD7B, 0xDD99,	0xDDB6, 0xDDD3, 0xDDF1, 0xDE0E, 0xDE2B, 0xDE49, 0xDE66, 0xDE84, 0xDEA2,	// 2036/1, 2, 3, 4, 5, 6, run6, 7, 8, 9, 10, 11, 12
			0xDEC0, 0xDEDE, 0xDEFB, 0xDF19,	0xDF36, 0xDF53, 0xDF71, 0xDF8E, 0xDFAB, 0xDFC9, 0xDFE6, 0xE004,			// 2037
			0xE022, 0xE040, 0xE05D, 0xE07B, 0xE098,	0xE0B6, 0xE0D3, 0xE0F1, 0xE10E, 0xE12B, 0xE149, 0xE166,			// 2038
			0xE184, 0xE1A2, 0xE1BF, 0xE1DD, 0xE1FB, 0xE218,	0xE236, 0xE253, 0xE271, 0xE28E, 0xE2AC, 0xE2C9, 0xE2E6,	// 2039/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0xE304, 0xE321, 0xE33F, 0xE35D, 0xE37A, 0xE398, 0xE3B5, 0xE3D3, 0xE3F1, 0xE40E, 0xE42C, 0xE449,			// 2040
			0xE466, 0xE484, 0xE4A1, 0xE4BF, 0xE4DC, 0xE4FA, 0xE518,	0xE535, 0xE553, 0xE571, 0xE58E, 0xE5AC,			// 2041
			0xE5C9, 0xE5E7, 0xE604, 0xE621, 0xE63F, 0xE65C, 0xE67A, 0xE697,	0xE6B5, 0xE6D3, 0xE6F0, 0xE70E, 0xE72C,	// 2042/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
			0xE749, 0xE767, 0xE784, 0xE7A1, 0xE7BF, 0xE7DC, 0xE7F9, 0xE817, 0xE835, 0xE852, 0xE870, 0xE88E,			// 2043
			0xE8AC, 0xE8C9, 0xE8E7, 0xE904, 0xE921, 0xE93F, 0xE95C, 0xE979, 0xE997,	0xE9B4, 0xE9D2, 0xE9F0, 0xEA0E,	// 2044/1, 2, 3, 4, 5, 6, 7, run7, 8, 9, 10, 11, 12
			0xEA2C, 0xEA49, 0xEA67, 0xEA84, 0xEAA1, 0xEABF, 0xEADC, 0xEAF9, 0xEB17,	0xEB34, 0xEB52, 0xEB70,			// 2045
			0xEB8E, 0xEBAB, 0xEBC9, 0xEBE6, 0xEC04, 0xEC21, 0xEC3F, 0xEC5C, 0xEC79, 0xEC97,	0xECB4, 0xECD2,			// 2046
			0xECF0, 0xED0D, 0xED2B, 0xED49, 0xED66, 0xED84, 0xEDA1, 0xEDBF, 0xEDDC, 0xEDF9, 0xEE17,	0xEE34, 0xEE52,	// 2047/1, 2, 3, 4, 5, run5, 6, 7, 8, 9, 10, 11, 12
			0xEE6F, 0xEE8D, 0xEEAB, 0xEEC8, 0xEEE6, 0xEF04, 0xEF21, 0xEF3F, 0xEF5C, 0xEF79, 0xEF97,	0xEFB4,			// 2048
			0xEFD2, 0xEFEF, 0xF00D, 0xF02A, 0xF048, 0xF066, 0xF083, 0xF0A1, 0xF0BF, 0xF0DC, 0xF0FA, 0xF117,			// 2049
			0xF134, 0xF152, 0xF16F, 0xF18D, 0xF1AA, 0xF1C8, 0xF1E5, 0xF203, 0xF221, 0xF23E, 0xF25C, 0xF27A, 0xF297	// 2050/1, 2, run2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
	};

	// table_months[escaped_years] represents escaped_lunar_months from 1881(including) to escaped_years.
	public static int[] table_months = new int[]{
			0x0000,
			0x000D, 0x0019, 0x0025, 0x0032, 0x003E, 0x004A, 0x0057, 0x0063, 0x006F,	0x007C,	// 1881 - 1890
			0x0088, 0x0095,	0x00A1,	0x00AD, 0x00BA, 0x00C6, 0x00D2, 0x00DF, 0x00EB, 0x00F8,	// 1891 - 1900
			0x0104, 0x0110,	0x011D, 0x0129, 0x0135, 0x0142, 0x014E, 0x015A, 0x0167, 0x0173,	// 1901 - 1910
			0x0180, 0x018C, 0x0198, 0x01A5, 0x01B1,	0x01BD, 0x01CA, 0x01D6, 0x01E3, 0x01EF,	// 1911 - 1920
			0x01FB, 0x0208, 0x0214, 0x0220, 0x022D, 0x0239, 0x0245, 0x0252,	0x025E, 0x026B,	// 1921 - 1930
			0x0277, 0x0283, 0x0290, 0x029C, 0x02A8, 0x02B5, 0x02C1, 0x02CE, 0x02DA, 0x02E6,	// 1931 - 1940
			0x02F3,	0x02FF, 0x030B, 0x0318, 0x0324, 0x0330, 0x033D, 0x0349, 0x0356, 0x0362,	// 1941 - 1950
			0x036E, 0x037B, 0x0387, 0x0393,	0x03A0, 0x03AC, 0x03B9, 0x03C5, 0x03D1, 0x03DE,	// 1951 - 1960
			0x03EA, 0x03F6, 0x0403, 0x040F, 0x041B, 0x0428, 0x0434,	0x0441, 0x044D, 0x0459,	// 1961 - 1970
			0x0466, 0x0472, 0x047E, 0x048B, 0x0497, 0x04A4, 0x04B0, 0x04BC, 0x04C9, 0x04D5,	// 1971 - 1980
			0x04E1, 0x04EE, 0x04FA, 0x0507, 0x0513, 0x051F, 0x052C, 0x0538, 0x0544, 0x0551,	// 1981 - 1990
			0x055D, 0x0569, 0x0576,	0x0582, 0x058F, 0x059B, 0x05A7, 0x05B4, 0x05C0, 0x05CC,	// 1991 - 2000
			0x05D9, 0x05E5, 0x05F1, 0x05FE, 0x060A, 0x0617,	0x0623, 0x062F, 0x063C, 0x0648,	// 2001 - 2010
			0x0654, 0x0661, 0x066D, 0x067A, 0x0686, 0x0692, 0x069F, 0x06AB, 0x06B7,	0x06C4,	// 2011 - 2020
			0x06D0, 0x06DC, 0x06E9, 0x06F5, 0x0702, 0x070E, 0x071A, 0x0727, 0x0733, 0x073F,	// 2021 - 2030
			0x074C, 0x0758,	0x0765, 0x0771, 0x077D, 0x078A, 0x0796, 0x07A2, 0x07AF, 0x07BB,	// 2031 - 2040
			0x07C7, 0x07D4, 0x07E0, 0x07ED, 0x07F9,	0x0805, 0x0812, 0x081E, 0x082A			// 2041 - 2049
	};

	public static int[] table_leap = new int[]{
			7, 0, 0, 5, 0, 0, 4, 0, 0, 2,	// 1881 - 1890
			0, 6, 0, 0, 5, 0, 0, 3, 0, 8,	// 1891 - 1900
			0, 0, 5, 0, 0, 4, 0, 0, 2, 0,	// 1901 - 1910
			6, 0, 0, 5, 0, 0, 2, 0, 7, 0,	// 1911 - 1920
			0, 5, 0, 0, 4, 0, 0, 2, 0, 6,	// 1921 - 1930
			0, 0, 5, 0, 0, 3, 0, 7, 0, 0,	// 1931 - 1940
			6, 0, 0, 4, 0, 0, 2, 0, 7, 0,	// 1941 - 1950
			0, 5, 0, 0, 3, 0, 10, 0, 0, 6,	// 1951 - 1960
			0, 0, 4, 0, 0, 3, 0, 7, 0, 0,	// 1961 - 1970
			5, 0, 0, 4, 0, 10, 0, 0, 6, 0,	// 1971 - 1980
			0, 4, 0, 10, 0, 0, 6, 0, 0, 5,	// 1981 - 1990
			0, 0, 3, 0, 10, 0, 0, 5, 0, 0,	// 1991 - 2000
			4, 0, 0, 2, 0, 7, 0, 0, 5, 0,	// 2001 - 2010
			0, 4, 0, 9, 0, 0, 6, 0, 0, 4,	// 2011 - 2020
			0, 0, 2, 0, 6, 0, 0, 5, 0, 0,	// 2021 - 2030
			3, 0, 7, 0, 0, 6, 0, 0, 5, 0,	// 2031 - 2040
			0, 2, 0, 7, 0, 0, 5, 0, 0, 2	// 2041 - 2050
	};

	static TSysFestivals m_vecSysFest = new TSysFestivals();
	static TUserFestivals m_vecMyFest = new TUserFestivals();
	static TUserFestivals m_vecDeadFest = new TUserFestivals();

	public static int MAX_MONTH_DAYS = 31;
	public static int MAX_LUNAR_MONTH_DAYS = 30;

	public static int[] gc_monthDay = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	public static int MIN_YEAR = -9999;
	public static uint MAX_YEAR = 9999;
	public static uint TOTALDAYS_BEFORE_1970 = 719528;	// total days before 1970.1.1 00:00:00

	// 날자수에 해당한 년,월,일 계산
    public static Date GetDateFromDays(int days)
    {
		Date dateSolar = new Date();
		if (days < 2)
			return dateSolar;

		int lo = days / 366, hi = days / 365 + 1;

		int nearDays = 0;
		while (lo < hi)
		{
			int mid = (lo + hi) / 2;
			nearDays = GetTotalDays(mid, 1);
			if (days == nearDays)
			{
				dateSolar.year = mid - 1;
				dateSolar.month = 12;
				dateSolar.day = 31;
				dateSolar.normalised = true;
				return dateSolar;
			}

			if (days < nearDays)
			{
				hi = mid;
				nearDays = 0;
			}
			else
				lo = mid + 1;
		}

		dateSolar.year = lo - 1;
		if (nearDays == 0)
			nearDays = GetTotalDays(dateSolar.year, 1);

		days -= nearDays;
		if (days <= 31)
		{
			dateSolar.month = 1;
			dateSolar.day = days;
			dateSolar.normalised = true;
			return dateSolar;
		}

		nearDays = 0;
		lo = 1;
		hi = days / 28 + 1;
		if (hi > 13)
			hi = 13;

		bool over = IsOverDay(dateSolar.year);
		for (; lo < hi; lo++)
		{
			int mid = gc_monthDay[lo - 1];
			if (lo == 2 && over)
				mid += 1;

			nearDays += mid;
			if (days == nearDays)
			{
				dateSolar.month = lo;
				dateSolar.day = mid;
				dateSolar.normalised = true;
				return dateSolar;
			}

			if (days < nearDays)
			{
				days -= (nearDays - mid);
				break;
			}
		}

		if (lo >= hi)
			days -= nearDays;

		dateSolar.month = lo;
		dateSolar.day = days;
		dateSolar.normalised = true;
		return dateSolar;
    }

	// total days since 1970.1.1 00:00:00
    public static int GetTotalDaysSince1970(Date date)
	{
		return GetTotalDays(date) - (int)TOTALDAYS_BEFORE_1970;
	}

	// 양력 년월에 해당한 총 일수
	public static int GetMonthDays( int year, int month )
	{
		if (month <= 0)
			return MAX_MONTH_DAYS;

		if (month > 12)
			return 0;

		if (month == 2)
		{
			if (IsOverDay(year))
				return gc_monthDay[month - 1] + 1;
		}

		return gc_monthDay[month - 1];
	}

	//올해전까지의 총 윤년 날자수
	public static int GetTotalOverDays( int year )
	{
		//4년마다 윤년, 100년마다 윤년제외, 400년마다 윤년포함
		if ( year == 0 )
			return 0;

		year --;
		return year/4 - year/100 + year/400;
	}

    //올해초부터 해당일까지의 날자수
    public static int GetNowDays( int year, int month, int day )
    {
		//전달까지의 총 일자 + 오늘 날자
		int i, iDays = 0;
		for (i = 1; i < month; i++)
		{
			iDays += GetMonthDays(year, i);
		}
		iDays += day;

		return iDays;
	}

	//지금까지의 총 날자수: year can be 0 - 9999
	public static int GetTotalDays(int year, int month)
	{
		//지금까지 날자수 = 작년 * 365 + 작년까지의 윤년 총일 + 이번달 1일까지의 날자
		if (year == 0 && month == 0)
			return 0;

		return (year) * 365 + GetTotalOverDays(year) + GetNowDays(year, month, 1);
	}

	public static int GetTotalDays(Date date)
	{
		if (!date.lunar)
			return GetTotalDays(date.year, date.month) + date.day;

		Date dateSolar = Lun2sol(date);
		if (dateSolar.year < 0)
			return 0;

		return GetTotalDays(dateSolar.year, dateSolar.month) + dateSolar.day;
	}

	//윤년 검사
	public static bool IsOverDay( int year )
	{
		//윤년이면 1, 아니면 0
		return ( year % 400 == 0 || ( year % 4 == 0 && year % 100 != 0 ) );
	}


    /* lun2sol(year, month, day, leap=False) -> (year, month, day, leap)
    Returns corresponding date in solar calendar. */
    public static Date Lun2sol(Date dateLunar)
    {
		int year = dateLunar.year - year_org;
		int year_upper = (sizeof(int)) * table_months.GetLength(0) / sizeof(int);
		if (((year < 0) || (year >= year_upper)) ||
			((dateLunar.month < 1) || (dateLunar.month > 12)) ||
			(dateLunar.leap && (dateLunar.month != table_leap[year])))
		{
			return new Date();
		}

		int months = table_months[year] + dateLunar.month - 1;
		if (dateLunar.leap || ((table_leap[year] > 0) && (dateLunar.month > table_leap[year])))
			months += 1;

		int days = table_days[months] + dateLunar.day - 1;
		if ((days < 0) || (days >= table_days[months + 1]))
		{
			//		assert(false);
			return new Date();
		}

		return GetDateFromDays(days + min_day); 
    }

    /* sol2lun(year, month, day) -> (year, month, day, leap)
    Returns corresponding date in lunar calendar. */
	// 양력을 음력으로 변환
    public static Date Sol2lun(Date dateSolar)
	{
		Date dateLunar = new Date(true);

		int days = GetTotalDays( dateSolar );
		if ( ( days < min_day ) || ( days > max_day ) )
			return dateLunar;

		days -= min_day;
		int size = sizeof(int) * table_days.GetLength(0) / sizeof(int);
		int month = __bisect(table_days, size, days);
		size = sizeof(int) * table_months.GetLength(0) / sizeof(int);
		int year = __bisect(table_months, size, month);
		int day = days - table_days[month] + 1;
		month = month - table_months[year] + 1;

		bool leap = false;
		if ( ( table_leap[year] != 0 ) && ( month > table_leap[year] ) )
		{
			month -= 1;
			leap = ( table_leap[year] == month );
		}
		dateLunar.year = year + year_org;
		dateLunar.month = month;
		dateLunar.day = day;
		dateLunar.leap = leap;
		return dateLunar;
	}

	private static int __bisect(int[] a, int size, int x)
	{
		int lo = 0, hi = size;
		while (lo < hi)
		{
			int mid = (lo + hi) / 2;
			if (x < a[mid])
				hi = mid;
			else
				lo = mid + 1;
		}
		return lo - 1;
	}

	//이번달 1일의 요일
	public static int GetFirstdayYoil( int year, int month )
	{
		// 0년 1월 1일은 일요일
		int totalDays;
		totalDays = GetTotalDays(year, month); //총 날자수 구함
		return ( totalDays - 1 ) % 7; // 일요일을 0으로 만들기 위해서 총 날자에서 1을 빼줌
	}

	// 음력 년월에 해당한 총 일수
	public static int GetLunarMonthDays(int year, int month, bool leap)
	{
		if (month <= 0)
			return MAX_LUNAR_MONTH_DAYS;

		if (month > 12)
			return 0;

		int months = GetTotalMonths(year, month, leap);
		if (months == -1)
			return MAX_LUNAR_MONTH_DAYS;

		return table_days[months] - table_days[months-1];
	}

	public static int GetTotalMonths(int year, int month, bool leap)
	{
		if ( ( year < year_org ) || ( year > gc_yearMax ) )
			return -1;
	
		int months = table_months[year-year_org] + month;
		if ( leap &&	( month == table_leap[year-year_org] ) )
			months += 1;
		if (!leap  && table_leap[year-year_org] != 0 && month > table_leap[year-year_org] )
			months += 1;
	
		return months;
	}

	/* Get corresponding unicode string of ganzi. 	// 간지문자렬 얻기
	params->
		index	 : [int] index from 1900
		sGanzi	 : [out] corresponding unicode string of ganzi. */
	public static void Getganzistr(int index, ref string sGanzi, int nGanOffset, int nZiOffset)
	{
		sGanzi  = MasterData.Text(CETextID.TXTID_CALENDAR_GANZI_START + (nGanOffset + index) % 10);
		sGanzi += MasterData.Text(CETextID.TXTID_CALENDAR_GANZI_START + 10 + (nZiOffset + index) % 12);
	}

	/* Get corresponding unicode string of nickname.
	params->
		index		 : [int] index from 1900
		sNickName	 : [out] corresponding unicode string of nickname. */
	// 년도의 별명문자렬 얻기
	public static void Getnicknamestr(int index, ref string sNickName, int offset)
	{
		sNickName = MasterData.Text(CETextID.TXTID_CALENDAR_NICKNAME_START + (index + offset) % 12);
	}
	
	public static string Getyoilstr(int nYoil, bool full)
	{
		uint text_id = full ? (uint)CETextID.TXTID_CALENDAR_FULL_WEEK_START : (uint)CETextID.TXTID_CALENDAR_SIMPLE_WEEK_START;

		return MasterData.Text(text_id + (uint)nYoil % 7);
	}
	public static int GetSeasonDay( int year, int month, int n )
	{
		if ( n > 300 )
		{
			for ( int i = 0; i < 2; i ++ )
			{
				Mds24Season season = MasterData.Singleton.m_24Seasons[month,i];
				if ( season.festivalId == n )
				{
					n = i;
					break;
				}
			}
		}
		if (( n < 0 ) || ( n > 1 ))
			return -1;

		Mds24Season season1 = MasterData.Singleton.m_24Seasons[month,n];
		
		if (season1.excepts.ContainsKey((uint)year))
			return (int)season1.excepts[(uint)year];

		// calculate rule:	[(Y%100)*0.2422+C]-[((Y%100)-B)/4]
		float c = ( year / 100 <= 19 ) ? season1.c[0] : season1.c[1];
		return (int)( ( year % 100 ) * 0.2422f + c ) - (int)( ( ( year % 100 ) - season1.b ) / 4 );
	}

	// 해당 양력날자가 24절기인가를 판정
	public static uint Is24Season( Date curDate_Solar, ref string szSeasonName, ref uint pFestID)
	{
		if ( pFestID != 0)
			pFestID = 0;

		for ( int i = 0; i < 2; i ++ )
		{
			if ( GetSeasonDay( curDate_Solar.year, curDate_Solar.month, i ) == curDate_Solar.day )
			{// this day is season day
				Mds24Season season = MasterData.Singleton.m_24Seasons[curDate_Solar.month,i];

				if (szSeasonName == string.Empty)
					szSeasonName = MasterData.Text(season.name);

				if ( pFestID == 0 )
					pFestID = season.festivalId;

				return season.festivalTypeId;
			}
		}
		return 0;
	}

	public static int RemainDays( Date curDate_Solar, Date fstDate_Solar, int deltaDays )
	{
		int remain = -1;
		if ( curDate_Solar.month == fstDate_Solar.month )
		{
			remain = fstDate_Solar.day - curDate_Solar.day;
			if (( remain >= 0 ) && ( remain < deltaDays ))
				return remain;
		}
		else if ( curDate_Solar.month < fstDate_Solar.month )
		{
			if ( fstDate_Solar.month == curDate_Solar.month + 1 )
			{
				remain = GetMonthDays( curDate_Solar.year, curDate_Solar.month ) - curDate_Solar.day + fstDate_Solar.day;
				if ( remain < deltaDays )
					return remain;
			}
		}
		else if (( curDate_Solar.month == 12 ) && ( fstDate_Solar.month == 1 ))
		{
			remain = GetMonthDays( curDate_Solar.year, curDate_Solar.month ) - curDate_Solar.day + fstDate_Solar.day;
			if ( remain < deltaDays )
				return remain;
		}
		return -1;
	}
	public static int RemainDays( Date curDate_Solar, Date curDate_Lunar, Date fstDate, int deltaDays )
	{
		if (( deltaDays > 20 ) || ( deltaDays < 0 ))
		{// too large delta
			deltaDays = 20;
		}

		if ( !fstDate.lunar )
		{// fstDate is solar.
			return RemainDays( curDate_Solar, fstDate, deltaDays );
		}
	
		// fstDate is lunar
		int remain;

		if ( curDate_Lunar.month == fstDate.month )
		{
			if (curDate_Lunar.leap == fstDate.leap)
			{
				remain = fstDate.day - curDate_Lunar.day;
				if (( remain >= 0 ) && ( remain < deltaDays ))
					return remain;
			}
			else if (!curDate_Lunar.leap && fstDate.leap)
			{
				Date fstDate1 = fstDate;
				fstDate1.year = curDate_Lunar.year;
				return RemainDays( curDate_Solar, Lun2sol( fstDate1 ), deltaDays );
			}
		}
		else if ( curDate_Lunar.month < fstDate.month )
		{
			if (( fstDate.month == curDate_Lunar.month + 1 ) && !fstDate.leap)
			{
				Date fstDate1 = fstDate;
				fstDate1.year = curDate_Lunar.year;
				return RemainDays( curDate_Solar, Lun2sol( fstDate1 ), deltaDays );
			}
		}
		else if (( curDate_Lunar.month == 12 ) && ( fstDate.month == 1 ) && !fstDate.leap)
		{
			Date fstDate1 = fstDate;
			fstDate1.year = curDate_Lunar.year + 1;
			return RemainDays( curDate_Solar, Lun2sol( fstDate1 ), deltaDays );
		}

		return -1;
	}

	public static int RemainDays( Date curDate_Solar, Date curDate_Lunar, MdsSysFest sysFest, int deltaDays )
	{
		int		remain;
		Date	fstDate_Solar = new Date();

		if (( deltaDays > 20 ) || ( deltaDays < 0 ))
		{// too large delta
			deltaDays = 20;
		}

		if ( !sysFest.bLunar )
		{// solar
			if ( curDate_Solar.month == sysFest.nMonth )
			{
                Date fstDate_Lunar = null;
                GetSysFestDate(curDate_Solar.year, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
				remain = fstDate_Solar.day - curDate_Solar.day;
				if (( remain >= 0 ) && ( remain < deltaDays ))
					return remain;
			}
			else if ( curDate_Solar.month < (int)sysFest.nMonth )
			{
				if ( sysFest.nMonth == curDate_Solar.month + 1 )
				{
                    Date fstDate_Lunar = null;
                    GetSysFestDate(curDate_Solar.year, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
					remain = GetMonthDays( curDate_Solar.year, curDate_Solar.month ) - curDate_Solar.day + fstDate_Solar.day;
					if ( remain < deltaDays )
						return remain;				
				}
			}
			else if (( curDate_Solar.month == 12 ) && ( sysFest.nMonth == 1 ))
			{
                Date fstDate_Lunar = null;
                GetSysFestDate(curDate_Solar.year + 1, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
				remain = GetMonthDays( curDate_Solar.year, curDate_Solar.month ) - curDate_Solar.day + fstDate_Solar.day;
				if ( remain < deltaDays )
					return remain;				
			}
		}
		else
		{
			if ( curDate_Lunar.month == sysFest.nMonth )
			{
				if ( sysFest.nDay <= 0 )
				{
                    Date fstDate_Lunar = null;
                    GetSysFestDate(curDate_Lunar.year, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
					return RemainDays( curDate_Solar, fstDate_Solar, deltaDays );
				}
				else if ( !curDate_Lunar.leap )
				{
					remain = sysFest.nDay - curDate_Lunar.day;
					if (( remain >= 0 ) && ( remain < deltaDays ))
						return remain;
				}
			}
			else if ( curDate_Lunar.month < (int)sysFest.nMonth )
			{
				if ( sysFest.nMonth == curDate_Lunar.month + 1 )
				{
                    Date fstDate_Lunar = null;
                    GetSysFestDate(curDate_Lunar.year, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
					return RemainDays( curDate_Solar, fstDate_Solar, deltaDays );
				}
			}
			else if (( curDate_Lunar.month == 12 ) && ( sysFest.nMonth == 1 ))
			{
                Date fstDate_Lunar = null;
				GetSysFestDate(curDate_Lunar.year + 1, sysFest, ref fstDate_Solar, ref fstDate_Lunar);
				return RemainDays( curDate_Solar, fstDate_Solar, deltaDays );
			}
		}

		return -1;
	}

	public static void GetSysFestDate( int year, MdsSysFest sysFest, ref Date pDateSolar, ref Date pDateLunar )
	{
		if ( !sysFest.bLunar )
		{
			Date dateSolar = new Date();
			if ( pDateSolar == null )
				pDateSolar = dateSolar;

			pDateSolar.year = year;
			pDateSolar.month = (int)sysFest.nMonth;
			pDateSolar.lunar = false;
			pDateSolar.leap = false;

			if ( sysFest.nWeek == 0 )
			{
				if ( sysFest.nDay > 300 )
					pDateSolar.day = GetSeasonDay( year, (int)sysFest.nMonth, sysFest.nDay );
				else
					pDateSolar.day = sysFest.nDay;
			}
			else
			{
				int firstDay = GetFirstdayYoil( year, (int)sysFest.nMonth );
				pDateSolar.day = ( sysFest.nDay >= firstDay ) ? ( sysFest.nDay - firstDay + 1 ) + ( (int)sysFest.nWeek - 1 ) * 7 : ( 8 - firstDay + sysFest.nDay ) + ( (int)sysFest.nWeek - 1 ) * 7;
			}
		
			if ( pDateLunar != null )
				pDateLunar = Sol2lun( pDateSolar );
		}
		else
		{
			Date dateLunar = new Date(true);
			if ( pDateLunar == null )
				pDateLunar = dateLunar;

			pDateLunar.year = year;
			pDateLunar.month = (int)sysFest.nMonth;
			pDateLunar.lunar = true;

			if ( sysFest.nDay <= 0 )
			{// last day of this month
				pDateLunar.leap = (sysFest.nMonth == table_leap[year - year_org]);
				pDateLunar.day = GetLunarMonthDays( year, (int)sysFest.nMonth, pDateLunar.leap);
			}
			else
			{
				pDateLunar.leap = false;
				pDateLunar.day = sysFest.nDay;
			}

			if ( pDateSolar != null)
				pDateSolar = Lun2sol(pDateLunar);
		}
	}

    // new (2017-4-5)
    public static void GetSysFestDate(MdsSysFest sysFest, int curYear, ref Date pDateSolar, ref Date pDateLunar)
    {
        GetSysFestDate(curYear, sysFest, ref pDateSolar, ref pDateLunar);

        if (pDateSolar.getYear() > curYear)
            GetSysFestDate(curYear - 1, sysFest, ref pDateSolar, ref pDateLunar);
        else if (pDateSolar.getYear() < curYear)
            GetSysFestDate(curYear + 1, sysFest, ref pDateSolar, ref pDateLunar);

        if (pDateSolar.year != curYear)
            pDateSolar.year = -1;
    }

    // old
	public static void GetSysFestDate( Date curDate, MdsSysFest sysFest, ref Date pDateSolar, ref Date pDateLunar )
	{
        int year = curDate.year;

        if (sysFest.bLunar)
        {
            Date lunar_date_begin_cur_year = Sol2lun(curDate);

            year = lunar_date_begin_cur_year.year;
            if ((sysFest.nMonth > lunar_date_begin_cur_year.month + 1))
            {
                year--;
            }
            else if ((sysFest.nMonth + 1 < lunar_date_begin_cur_year.month))
            {
                year++;
            }

            GetSysFestDate(year, sysFest, ref pDateSolar, ref pDateLunar);
        }
        else
        {
            GetSysFestDate(year, sysFest, ref pDateSolar, ref pDateLunar);

            if (pDateSolar != null)
            {
                if (pDateSolar.year < curDate.year)
                    GetSysFestDate(year + 1, sysFest, ref pDateSolar, ref pDateLunar);
                else if (pDateSolar.year > curDate.year)
                    GetSysFestDate(year - 1, sysFest, ref pDateSolar, ref pDateLunar);

                if (pDateSolar.year != curDate.year)
                    pDateSolar.year = -1;
            }
        }
	}

	public static TUserFestivals GetMyFestivals( Date curDate_Solar, Date curDate_Lunar )
	{
		m_vecMyFest.Clear();

		TFestivals myFest = GameData.Singleton.getMyFestivals();

		Dictionary<uint, Festival>.Enumerator enumerator = myFest.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<uint, Festival> itFind = enumerator.Current;
			USER_FEST_INFO info = new USER_FEST_INFO();
			info.type_festival = (uint)CEMacro.SELFFEST_TYPE_ID;
			if (itFind.Value.date.lunar)
			{
				if ((itFind.Value.date.month == curDate_Lunar.month) && (itFind.Value.date.day == curDate_Lunar.day) && (itFind.Value.date.leap == curDate_Lunar.leap))
				{
					info.id_festival = itFind.Key;
					info.name = itFind.Value.name;
					info.year_defined = itFind.Value.date.getYear();
					info.date_main = itFind.Value.date.Clone();
					info.date_sub = curDate_Solar;
					m_vecMyFest.Add(info);
				}
			}
			else if ((itFind.Value.date.month == curDate_Solar.month) && (itFind.Value.date.day == curDate_Solar.day))
			{ 
				info.id_festival = itFind.Key;
				info.name = itFind.Value.name;
				info.year_defined = itFind.Value.date.getYear();
				info.date_main = itFind.Value.date.Clone();
				info.date_sub = new Date(true);
				m_vecMyFest.Add(info);
			}
		}
		return m_vecMyFest;
	}

	public static TUserFestivals GetDeadFestivals( Date curDate_Solar, Date curDate_Lunar )
	{
		m_vecDeadFest.Clear();

		List<uint> myRooms = GameData.Singleton.getMyRooms();

		for (int nIndex = 0; nIndex < myRooms.Count; nIndex++)
		{
			for (int i = 0; i < 2; i++)
			{
				// i==0: the main dead, i==1: the sub dead
				DeadInfo di = i == 0 ? GameData.Singleton.getDeadInfo(myRooms[nIndex], DeadInfoType.DIT_MAIN) : GameData.Singleton.getDeadInfo(myRooms[nIndex], DeadInfoType.DIT_SECOND);
				string deadName = di.sDeadSurname + di.sDeadName;

				if (!deadName.Equals(string.Empty))
				{
					USER_FEST_INFO info = new USER_FEST_INFO();
					RoomInfo roomInfo = GameData.Singleton.getRoom(myRooms[nIndex]);
					string room_ui_name = string.Empty;

					if (roomInfo != null && di.DeadDay.year != 0 && di.DeadDay.month != 0 && di.DeadDay.day != 0)
					{
						if (di.DeadDay.lunar)
						{
							if ((di.DeadDay.month == curDate_Lunar.month) && (di.DeadDay.day == curDate_Lunar.day) && (di.DeadDay.leap == curDate_Lunar.leap))
							{
								info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2);
								info.id_festival = myRooms[nIndex];		// this is special. We save room_id in the id_festival;

								room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
								info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_MEMORIALDAY), deadName, room_ui_name);

								info.year_defined = di.DeadDay.getYear();
								info.date_main = di.DeadDay;
								info.date_sub = curDate_Solar;
								m_vecDeadFest.Add(info);
							}
						}
						else if ((di.DeadDay.month == curDate_Solar.month) && (di.DeadDay.day == curDate_Solar.day))
						{
							info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2);
							info.id_festival = myRooms[nIndex];

							if (room_ui_name.Equals(string.Empty))
								room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
							info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_MEMORIALDAY), deadName, room_ui_name);

							info.year_defined = di.DeadDay.getYear();
							info.date_main = di.DeadDay;
							info.date_sub = new Date(true);
							m_vecDeadFest.Add(info);
						}
					}

					if (roomInfo != null && di.Birthday.year != 0 && di.Birthday.month != 0 && di.Birthday.day != 0)
					{
						if (di.Birthday.lunar)
						{
							if ((di.Birthday.month == curDate_Lunar.month) && (di.Birthday.day == curDate_Lunar.day) && (di.Birthday.leap == curDate_Lunar.leap))
							{
								info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2 + 1);
								info.id_festival = myRooms[nIndex];

								if (room_ui_name.Equals(string.Empty))
									room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
								info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_BIRTHDAY), deadName, room_ui_name);

								info.year_defined = di.Birthday.getYear();
								info.date_main = di.Birthday;
								info.date_sub = curDate_Solar;
								m_vecDeadFest.Add(info);
							}
						}
						else if ((di.Birthday.month == curDate_Solar.month) && (di.Birthday.day == curDate_Solar.day))
						{
							info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2 + 1);
							info.id_festival = myRooms[nIndex];

							if (room_ui_name.Equals(string.Empty))
								room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
							info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_BIRTHDAY), deadName, room_ui_name);

							info.year_defined = di.Birthday.getYear();
							info.date_main = di.Birthday;
							info.date_sub = new Date(true);
							m_vecDeadFest.Add(info);
						}
					}
				}
			}
		}

		return m_vecDeadFest;
	}
	
	public static TSysFestivals GetSysFestivals( Date curDate_Solar, Date curDate_Lunar )
	{
		m_vecSysFest.Clear();

		// 양력명절 조사
		for (int nIndex = 0; nIndex < MasterData.Singleton.m_SysSolarFests.Count; nIndex++)
		{
			if (MasterData.Singleton.m_SysSolarFests[nIndex].key == curDate_Solar.month)
			{
				Date date_solar = new Date();
                Date date_lunar = null;
				GetSysFestDate(curDate_Solar.year, MasterData.Singleton.m_SysSolarFests[nIndex].sysFest, ref date_solar, ref date_lunar);
				if (date_solar == curDate_Solar)
					m_vecSysFest.Add(MasterData.Singleton.m_SysSolarFests[nIndex].sysFest);
			}
		}
		
		// 음력명절 조사
		for (int nIndex = 0; nIndex < MasterData.Singleton.m_SysLunarFests.Count; nIndex++)
		{
			if (MasterData.Singleton.m_SysLunarFests[nIndex].key == curDate_Lunar.month)
			{
                Date date_solar = null;
				Date date_lunar = new Date();
                GetSysFestDate(curDate_Lunar.year, MasterData.Singleton.m_SysLunarFests[nIndex].sysFest, ref date_solar, ref date_lunar);
				if (date_lunar == curDate_Lunar)
					m_vecSysFest.Add(MasterData.Singleton.m_SysLunarFests[nIndex].sysFest);
			}
		}
		
		return m_vecSysFest;
	}

    // Return all festivals corresponded with selffest_type_id key 
    // in year of that date
	public static TUserFestivals GetMyFestivalsAll( int year )
	{
		m_vecMyFest.Clear();

		TFestivals myFest = GameData.Singleton.getMyFestivals();

		TFestivals.Enumerator enumerator = myFest.GetEnumerator();
		while (enumerator.MoveNext())		
		{
			KeyValuePair<uint, Festival> it = enumerator.Current;
		
			USER_FEST_INFO info = new USER_FEST_INFO();
			info.type_festival = CEMacro.SELFFEST_TYPE_ID;
			info.id_festival = it.Key;
			info.name = it.Value.name;
            GetFestInfoFromFestDate(it.Value.date.Clone(), year, info);
// 			GetUserFestInfoFromDate(it.Value.date.Clone(), curDate_Solar, info);
			m_vecMyFest.Add(info);
		}

		return m_vecMyFest;
	}

// 	public static TUserFestivals GetDeadFestivalsAll( Date curDate_Solar )
	public static TUserFestivals GetDeadFestivalsAll( int year )
	{
		m_vecDeadFest.Clear();


		List<uint> myRooms = GameData.Singleton.getMyRooms();
		for (int nIndex = 0; nIndex < myRooms.Count; nIndex++)
		{
			uint id = myRooms[nIndex];
			for (int i = 0; i < 2; i++)
			{
				// i==0: the main dead, i==1: the sub dead
				DeadInfo di = i == 0 ? GameData.Singleton.getDeadInfo(id, DeadInfoType.DIT_MAIN) : GameData.Singleton.getDeadInfo(id, DeadInfoType.DIT_SECOND);
				string deadName = di.sDeadSurname + di.sDeadName;

				if (!deadName.Equals(string.Empty))
				{
					RoomInfo roomInfo = GameData.Singleton.getRoom(id);
					string room_ui_name = string.Empty;

					if (roomInfo != null && di.DeadDay.year != 0 && di.DeadDay.month != 0 && di.DeadDay.day != 0)
					{
                        USER_FEST_INFO info = new USER_FEST_INFO();

						info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2);
						info.id_festival = id;

						room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
						info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_MEMORIALDAY), deadName, room_ui_name);

                        GetFestInfoFromFestDate(di.DeadDay, year, info);
// 						GetUserFestInfoFromDate(di.DeadDay, curDate_Solar, info);
						m_vecDeadFest.Add(info);
					}

					if (roomInfo != null && di.Birthday.year != 0 && di.Birthday.month != 0 && di.Birthday.day != 0)
					{
                        USER_FEST_INFO info = new USER_FEST_INFO();

						info.type_festival = (uint)(CEMacro.DEADFEST_TYPE_ID + i * 2 + 1);
						info.id_festival = id;

						if (room_ui_name.Equals(string.Empty))
							room_ui_name = GameData.Singleton.GetRoomUIName(roomInfo);
						info.name = string.Format(MasterData.Text(CETextID.TXTID_FORMAT_FESTTABLE_BIRTHDAY), deadName, room_ui_name);

                        GetFestInfoFromFestDate(di.Birthday, year, info);
// 						GetUserFestInfoFromDate(di.Birthday, curDate_Solar, info);
						m_vecDeadFest.Add(info);
					}
				}
			}
		}

		return m_vecDeadFest;
	}

	public static bool CheckDate(Date date)
	{
		if (date.getYear() > MAX_YEAR || date.getYear() < MIN_YEAR)
			return false;

		if (date.getMonth() < 0 || date.getMonth() > 12 || date.getDay() < 0)
			return false;

		int month_days;
		if (!date.isLunar())
			month_days = GetMonthDays(date.getYear(), date.getMonth());
		else
			month_days = GetLunarMonthDays(date.getYear(), date.getMonth(), date.isLeap());
	
		return (date.getDay() <= month_days);
	}

    public static void GetFestInfoFromFestDate( Date fest_date, int cur_solar_year, USER_FEST_INFO pInfo )
    {
        if (fest_date.lunar)
        {
            fest_date.setYear(cur_solar_year);

            Date fest_solar = Lun2sol(fest_date);
            if (fest_solar.getYear() > cur_solar_year)
                fest_date.setYear(cur_solar_year - 1);

            pInfo.date_main = fest_date;
            pInfo.date_sub = Lun2sol(fest_date);
        }
        else
        {
            pInfo.year_defined = cur_solar_year;
            fest_date.setYear(cur_solar_year);

            pInfo.date_main = fest_date;
            pInfo.date_sub = new Date(true);
        }
    }

	public static void GetUserFestInfoFromDate( Date date_fest, Date date_cur_year, USER_FEST_INFO pInfo )
	{
		// < yjp: modified. /yjp>
		if ( date_fest.lunar )
		{
// 			Date solar_date_begin_cur_year = new Date( date_cur_year.year, 1, 1 );
// 			Date lunar_date_begin_cur_year = Sol2lun(solar_date_begin_cur_year);
            Date lunar_date_begin_cur_year = Sol2lun(date_cur_year);

			Date date_lunar = date_fest;
//			pInfo.year_defined = date_lunar.year;
//			date_lunar.year = date_cur_year.year;
            date_lunar.year = lunar_date_begin_cur_year.year;
			if ( (date_fest.month > lunar_date_begin_cur_year.month + 1) )
			{
				date_lunar.year --; 
			}
            else if ( (date_fest.month + 1 < lunar_date_begin_cur_year.month) )
            {
                date_lunar.year ++; 
            }

			pInfo.date_main = date_lunar;
			pInfo.date_sub = Lun2sol( date_lunar );
		}
		else
		{
			int year_solar = date_cur_year.year;

			Date date_solar = date_fest;
			pInfo.year_defined = date_solar.year;

			date_solar.year = year_solar;

			pInfo.date_main = date_solar;
			pInfo.date_sub = new Date(true);
		}
	}

	public static void GetYearGanzi( int year, ref string sYearGanzi, ref string sYearNick )
	{
		if ( year >= gc_yearMin )
		{
			Getganzistr(year - gc_yearMin, ref sYearGanzi, year_min_gan, year_min_zi);
			Getnicknamestr(year - gc_yearMin, ref sYearNick, year_min_nick);
		}
		else
		{
			int gan = year_min_gan;
			int zi = year_min_zi;
			int nick = year_min_nick;
			int diff = gc_yearMin - year;
			while ( diff > 0 )
			{
				if ( gan > 0 )
					gan -= 1;
				else
					gan = 9;

				if ( zi > 0 )
					zi -= 1;
				else
					zi = 11;

				if ( nick > 0 )
					nick -= 1;
				else
					nick = 11;

				diff --;
			}
			Getganzistr(0, ref sYearGanzi, gan, zi);
			Getnicknamestr(0, ref sYearNick, nick);
		}
	}

	public static void GetMonthGanzi( int year, int month, ref string sMonthGanzi )
	{
		if ( year >= gc_yearMin )
		{
			int months_offset = (year - gc_yearMin) * 12 + month - 1;
			Getganzistr(months_offset, ref sMonthGanzi, month_min_gan, month_min_zi);
		}
		else
		{
			int gan = month_min_gan;
			int zi = month_min_zi;
			int diff = gc_yearMin - year;

			while ( diff > 0 )
			{
				gan -= 2;
				if ( gan < 0 )
					gan += 10 ;

				diff --;
			}

			Getganzistr(month - 1, ref sMonthGanzi, gan, zi);
		}
	}

	public static void GetDayGanzi( int days, ref string sDayGanzi )
	{
		if ( days >= days_min )
			Getganzistr(days - days_min, ref sDayGanzi, day_min_gan, day_min_zi);
		else
		{
			int gan = day_min_gan;
			int zi = day_min_zi;
			int diff = (days_min - days) % 60;
			while ( diff > 0 )
			{
				if ( gan > 0 )
					gan -= 1;
				else
					gan = 9;

				if ( zi > 0 )
					zi -= 1;
				else
					zi = 11;

				diff --;
			}
			Getganzistr(0, ref sDayGanzi, gan, zi);
		}
	}
}
