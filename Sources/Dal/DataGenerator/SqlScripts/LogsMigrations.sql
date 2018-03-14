﻿USE [{0}]

CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201802140517394_vasiliy.stan_7c2229cb57c14bf3bfb07099cc32c239', N'Logs.Migrations.Configuration',  0x1F8B0800000000000400CD55CB6EDB3010BC17E83F08BCC7B49D4B1BC809523B2902C47151B9B933D24A21CA57B5AB20FAB61EFA49FD85AE6CC9AF348E13F4D023C9E5ECECEC70F9FBE7AFF8ECD19AE8014AD4DE8DC4A0D71711B8D467DA152351517EF4419C9DBE7F175F64F631BAEDE28E9B38BEE97024EE89C2899498DE8355D8B33A2D3DFA9C7AA9B752655E0EFBFD8F723090C01082B1A228FE5A39D216160B5E8EBD4B2150A5CCD46760B0DDE79364811ADD280B18540A23716103D5223A375A71F2044C2E22E59C27454CEDE41B4242A5774512784399791D80E37265105ACA27EBF043D9F7870D7BB9BED841A51592B7AF041C1CB772C8DDEB6F1255ACE462C12E5858AA595052DA41B9506E24AE7D81CD1E3C92887693369A8D4DD92A4565C542C9750BE4B2075DAFE433CD8AA72A0476CD46F3DA9D2859766E7C94BCBE3EBBC49029EE2B7395897CA90AD839E5D4CCF45297481345EA4E35561867F649D8964C9B12B4F82B0D766A8DDBBC2FBBF70991658888BE94FE41670D89A44602DB6B027AC90F33361A1CAD03A6CAE91C90E6FE3BB0DDD84DC39DD7F0FF38532266E6007BFEA53D7B0CF854EC586E0E947802A80B46DF182F0ED2A68A35681773E572CFD20628A94E8036B97621DD714B760AA432E6795E92CE554A7C9C0222DB4144B7CA548B197507D9959B55142A3A47047B67EAED9AF6E75FBCB26DCEF12C342BFC1725304DCD25C0CC7DAAB4C956BC2F9753521E00D1B4EC33F0FEC21D3C7219AEA8574837DE1D08D4CA3781002E63A7CFC106C3603873897A80B770E379760D854AEBEECD3C0FF27223B6658F275A15A5B2D862ACEF379FA46C7EC9D33F6BF277F357070000 , N'6.2.0-61023')
GO

CREATE TABLE [dbo].[FrontendWebLogs] (
    [Id] [int] NOT NULL IDENTITY,
    [Date] [datetime2](7) NOT NULL,
    [Thread] [nvarchar](255) NOT NULL,
    [Level] [nvarchar](50) NOT NULL,
    [Logger] [nvarchar](255) NOT NULL,
    [Message] [nvarchar](4000) NOT NULL,
    [Exception] [nvarchar](2000) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.FrontendWebLogs] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Seeding] (
    [Id] [int] NOT NULL IDENTITY,
    [IsSeed] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Seeding] PRIMARY KEY ([Id])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201802140522109_vasiliy.stan_6714e9077a3d4050997998650cf8784c', N'Logs.Migrations.Configuration',  0x1F8B0800000000000400ED59CD6EE33610BE17E83B083A672DC769D06D20EF22EB2445D03809A2647BA6A5B14294225592F2DACFB6873E525FA143FD5B921DD94E82A2287CB1479C6F7E38BFF2DFDFFF723F2F23662D402A2AF8D83E1E0C6D0BB82F02CAC3B19DE8F9878FF6E74F3FFEE05E06D1D2FA5A9C3B31E79093ABB1FDAC757CE638CA7F8688A841447D299498EB812F228704C2190D87BF38C7C70E20848D5896E53E245CD308D21FF87322B80FB14E089B8A0098CAE9F8C44B51AD5B12818A890F63FB8230DB3A6794A0680FD8DCB608E742138D8A9D3D29F0B4143CF4622410F6B88A01CFCD0953902B7C561DEFABFB706474772AC602CA4F9416D18E80C727B9339C26FB5E2EB54B67A1BB2ED1AD7A65AC4E5D36B6AFD0191A78F03BCC6E44A852F7DA5653F0D98449C3947A77D0C57364E193A33200304ECCE7C89A244C2712C61C122DCD89FB64C6A8FF1BAC1EC51FC0C73C61ACAE21EA88CFD60848BA972206A9570F30CFF5BE0E6CCB59E7739A8C255B8D27B3E29AEB93916DDDA2703263500640CD624F0B09BF0207493404F7446B90DC6040EAC296F486AC0BE42AA499EF8F18CD1D02B7833C3E4B20A5D218B89874B63525CB1BE0A17E1EDBA3D353DBBAA24B080A4A8EFBC429E628326999EC2EF70616260A368A3D1DBE8954118620DFDFDA29284542D822F8A7E1F04D2CBE5C9AAA9626F666A3DF48F683F896276B21FC0BE54462609B629B4889557E354DF153E16B4A7DECD6688F7C9A88284E3434B2D975AA42B5B57C790041AF9A551EFCBF50356429E39A3206846040F80BD1B4FD7A307E34A1A84F95D7CAD060A93B6E093B727E512A8FE47575334C0F7477C3B2AD4A95ACDF77B6A796179AB0C60969EAB5E06A21D6E583D2DA6A2671B2A1A4185E9C0DD38B3B25718C326BD34C4EB1BC6C94997CF0766FF95186E1F8AAA3F397DA9692307AB0F8359E9ABC0CE08A4AA5B179911931113009A2D6B1FADD6E707021A9FBFA9A0954B9BFE033DF8B9EDA3D7B34412A475EA16D11E6416A26942A6D187B5A30E97C4918911DF939112C89F8A61CDFC69D8D0675FE8CD21FA1980BEA1805AD3F4ADEE5EB203969078CBC67AF81E4B4FE286507AEC394C4FE38B57E5A47AA91FB63D5FB631DAC4E6FA3B94E23F29AF1EDB402BCD11B9A79D32BAB8AEAB56B3655B56DF714DAC2FB36795374AA35849CF6CE37D12AFECD23A5F4B209348ABD9B17DE97F7D95625CE8ED816BA6741035385BD95D2100DCC8181F7279B308AF65607A684D339289D0D3238511E8F1A1BF2BF675B75940AD81E2BEBBB4F66D478F8C5D96BC7A1BCBE3506F8DDBC0319998B049F662F567E3E7089E40B22FD6722DB8BD5213B6227AAD9120F5A015F4DD7C686D7899BED78872E70DD2AEF03DDDECFA4F8B628282F6E5307CCEF9DEBD57F23BFD6979D19D5AFBAE8B467E69D56997CC9E8B9CF645D00CBC44CA03D9905ADDDA8C7D2B3496AED5997A8725FDA73276AB73FD7A9BFF4752F40D1B08230AF8039F826D72AD0E2CC359F8BE2A6D1BABA46C5915645D0040B2C39979ACE89AFF1B18F4522B5E82B6189C9ED6806C135BF4B34A6D4B95210CDD8DA36ED3ADBE5A78BDFBACEEE5D5A2DD46B98806A52D323EEF89784B2A0D4FBAA238C37409880CDB30BB5F2B4C9B2705522DD0ADE132877DF05C4187D989B8F10C50CC1D41DF7C802F6D1ED49C10D84C45F1553CC6690972F62DDEDEE0525A12491CA312A7EF3478663FEC9F8F40F6E282CC9FB180000 , N'6.2.0-61023')
GO

CREATE TABLE [dbo].[CoreBitconServiceLogs] (
    [Id] [int] NOT NULL IDENTITY,
    [Date] [datetime2](7) NOT NULL,
    [Thread] [nvarchar](255) NOT NULL,
    [Level] [nvarchar](50) NOT NULL,
    [Logger] [nvarchar](255) NOT NULL,
    [Message] [nvarchar](4000) NOT NULL,
    [Exception] [nvarchar](2000) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_dbo.CoreBitconServiceLogs] PRIMARY KEY ([Id])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201802191430194_vasiliy.stan_62864fe486ad40509a334f32ac79ba87', N'Logs.Migrations.Configuration',  0x1F8B0800000000000400ED5ADB6EE3460C7D2FD07F10F49CB51CA78B6E03791789931441E3248892EDF358A29D41A5197766E4B5BF6D1FFA49FD857274BF5B762E6D01232F36353CBC88E49074FEFEFE97FD651DF8C60A84A49C8DCDE3C1D03480B9DCA36C31364335FFF0C9FCF2F9C71FEC4B2F581B5FD37327FA1C723239369F955A9E5A96749F21207210505770C9E76AE0F2C0221EB746C3E12FD6F1B1050861229661D80F21533480E80B7E9D70E6C25285C49F720F7C99D0F18913A11AB72400B9242E8CCD1BBE90A671E65382B21DF0E7A64118E38A28D4ECF44982A304670B678904E23F6E9680E7E6C49790687C9A1FEFABFC70A495B772C614CA0DA5E2C18E80C7278937AC2AFB5E3E35336FA1BF2ED1AF6AA3AD8E7C3636275CC039552E670E88157541FB2FF2B26954C59F4E7CA15963270FDA598F0CFDF1288B070C1BFD77644C425F8502C60C4225089EBB0F673E757F83CD23FF03D89885BE5FD41735C667250292EE055F82509B079827565C7BA66195F9AC2A63C656E089ADB966EA64641AB7289CCC7CC8C2A160B9A3D0D85F8181200ABC7BA21408A6312072684D7A45D60572A5D2F4E7470CEE0681DD208FCF0248A6348631E6A0694CC9FA06D8423D8FCDD1C78FA67145D7E0A59404F789514C59645222DC5DEE0DAC7434B48AFD387C13A97CB100F1FED64E414AB2800EC13F0D876F62F1E55A17B928CDDB8D7E23D90FFC5B92ACA9F073CA88C0C0D6B53714028BFE661AE147C24B4A7D6AD6688F7C9AF060192AA864B36DE565ABB3985D616557C0BCDF61D6BF8C35311D0AD8A1801D0AD8A180BD77017300BC7E552B3B792855555952FB268B02CE7D206C4B3C75BF208C204528EA9367B6D43458AB86F7842346F2AA6412CB6575634C075457076E1AB942F118D3D16ED73C521551B9E21AC09B6FCE2DB0DACF517ED7E00A71DCE4E6CCA1F92067C5935C3AF1592D239F3D25CB25CA2C8C8009C570E2F96FF2C1D97D4C0A620CCB950DD352A66D260903142B6CE5A97E931E5C512115DE90644674904DBCA076AC183E2D0E4E257545483553F3979072EBCF79C4760550052BF7EA151A1A60DE453643A65FE7DC58038BE674E213D1501526DC0F03D65659BAB8E396A4C81F53FA23A4FD481123A5F54749BA8B224842DA0123E9154A2009AD3F4A76F3176132627F9CC23D5E442A90FB6315EFE52258915E47B3AD4AFC5583DDAA457BE546AAA652AF44ABD6C99D53ACB18CEE9E5CFD600E69B515E3905665AC7F27ADD23E61E774CADB88DD73A883F76D1227ED3B4B0809ED9D5F45ADCFAA1EC9A467FD56A5AFB2931E67FBBEBDD6F4C4474C03DDB3A29E6E789C8D54100CF48181F3A73FF129DA9B1F981246E720553C96E084783CAA2CF0FF3BCB744B4ACFDF7BA3FEEED316D57EDE3A4FED386A1777411E7ED6BFD48CF4EB0497C63FFFFCFCC2D5105B11E13E13515F97BC64F3D388AA773F2F5AECBC9AAE95BD4D236EBCB979E95AA659E57DA0EB5B17C1BFAD52CAD61DC90B66F26D5BDF43AA1D52ED906AAF9D6A2D4DD5FF36BFCABBC21955AFBA27ACEF83F6D804266BB49DD681710B862563C6D1B6D89A9635E36E9BC336659A8F35A9515B45F6D831B6492D3C6B1295AD27F75C41D65B60DB2AFE638A7D01922E7208FD6F2A0C5C9DFE39687AE69ACD791A7C685D51A3F448AD482982359F9C0945E7C455F8D8C5BA1559F495F8A12E37C10CBC6B76172ACCF233292198F9A5FDB86D75CB8FF6AC659DEDBBA880C9D73001D5A4FADABA63E721F5BD4CEFAB86CC6A81D03994243C6AE5289DF88B4D8674CB594FA0C47D17B0C4E8C372F108C1D2473079C71CB2827D747B9270030BE26ED249A61D64FB8B28BBDDBEA064214820138C9C1FBF620C7BC1FAF33FD4A1A0CC9F250000 , N'6.2.0-61023')
GO
