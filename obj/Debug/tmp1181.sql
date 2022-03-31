ALTER TABLE [dbo].[Chatlogs] ADD [AppointmentId] [int] NOT NULL DEFAULT 0
CREATE INDEX [IX_AppointmentId] ON [dbo].[Chatlogs]([AppointmentId])
ALTER TABLE [dbo].[Chatlogs] ADD CONSTRAINT [FK_dbo.Chatlogs_dbo.appointmentlists_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [dbo].[appointmentlists] ([id]) ON DELETE CASCADE
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202111160553174_addAppointmentID', N'Lawave.Migrations.Configuration',  0x1F8B0800000000000400ED5D5B8F24B5157E8F94FFD0EAA7245AA667767381D50C6898DD41A3EC8DED5D94B795A7CAD353505DD5A9CBEC8CA2484449048110445058240824CA0B0902B180100485FC197676F6297F21AE4B57F95AB6AB5CD55DA8352FD32EFBD83EE7B37D7C7CECF3BFAFFFBBF9D4F1D41D1CC120747C6F6BB8B1B63E1C40CFF26DC79B6C0DE3E8E0B1C7874F3DF9FDEF6D5EB6A7C783E7E6F92E24F950492FDC1A1E46D1ECE268145A87700AC2B5A963057EE81F446B963F1D01DB1F9D5F5F7F62B4B1318288C410D11A0C366FC65EE44C61FA03FDDCF13D0BCEA218B8577D1BBA619E8EBE8C53AA836B600AC319B0E0D6F00AB80B8EE05A967138D8761D801A3186EEC170003CCF8F40849A78F17608C751E07B93F10C2500F7D6C90CA27C07C00D61DEF48B6576D55EAC9F4F7A312A0BCE49597118F9534D821B1772B68CE8E2B5983B2CD896B2743A73E171D2ED947B5BC3EDD9CC77BC680ABDE866EC2226D0955EDC7183243FC5E335AAE0B941F6F95C0107849AE4EFDC602776A338805B1E8CA300B8E70637E27DD7B17E0E4F6EF92F406FCB8B5D176F256AE78DC09FC1203AC91B19C6D3E1206B04121F02E17070151C5F81DE243ADC1AA27F87835DE718DAF3945C9EB73D076116158A8218FD1C55D6314DF8DB721D510CC3D62BB90BEDF63B721807AD57721038ADD71182C8701D9B236C90E1E997D1E4169D60430F9423C875C2487DE85105CD0E3DF48D48C07876131EE44D776C86B123BA20CD6AA780E59E175D383F1C5C4395837D17165CC57A3E8EFC003E033D188008DA374014C1004D9F7B364CB92813AB0BEE9EC060DBB27CB4A4EC492BAE26364B79262226C117EA4F5C8E79B41EE1D21BE75F351B648110EE79077EEBA323A928A9A2F58A02F83CB4B225AEE59A32645C9F395E77B52131075AA871D3F1DC6123516DDA8D2C64760BE96CF39297D070CD7E4BC74510F10B6A0E067884D4B81A2DD8F39C28C9ABDF806BE0C899A4D354D5AC331CDC846E9A2B3C7466F9D82772DC611680DDC09FDEF45D9A149DF1CE2D104C60B270F92AB9C77E1C581ADD20E63B6E37881C55DDA8CCC874A33A37AF1B9BA37255AD5C6B29D1A8AEB444B1D53ACB2ACDC0712BE627B40D519A9F741764108677FDA07D35F7465ED118B8A6D5448EDC42A4384E632F657B56D7D33E1A17C0D366D0C4F7272EDCBBD4BE8AEE046194FC6B0004B245AAA38AC243BF7D591FC1C03938B981AAF2B329A6CB1A779D4EAB1B43AB83B1934DB84D07CEECD0F7AA2076C108C2C0494AB76D9E7851E0DB71376AF52CF00F60982C8AA06A4130545922A51D3F6C7F9C1E00AB9B8AD0E48F7E39078E057079D5C571909AF2B04D2763E6D324182185C11D5B4859D0DA2338E6956C46DB6C4FCD9E6B9C6A6AF65C9BD5DC2D041054F5017DBE4329AF4CF3993CA20D029B51776F90957EDA05D60B12EE177964CD176414F44194BB5E472EA3F9316B3EB72757C84CE29E5466647A529D5BB72739B56394E640CF82955D297249FB22C829EA8C287B3DB93CE3FBF67694999E84082B33C92026CA29C098307BBD9DF48EEFBA73EB9670335D661277A63AA7603B2DCC6E623F9D4E5E9A9B695466B5936ED7620D52B9E036679470D94B4ED1DA376ED55BB1046BAD78696B005B6CF9D2C36E517005E0451EB9B48EE1C6BA0B1FCD324DA72D032D837A9181966E66B58196CEDD6841A1FAA43A2E8962AB51B932D0AE0CB42B03AD291353DB86C04E2D740B32C5183A2AA41754B5834593560D434B6A7547440B70BB1B4E6AEDADD87092AD956E38C9EC06F403BC677A2A425972A5257C477577638614FE1895DA5D4CABEF0646A8B427FC015D6B8452564DF5014A155C8DCF76C767681D22AD55A2AA99D1396C380341BA2C7754DD24802D54D3604E327482408F63B5F38626C3B8B0E76B8FE3A2E46A20B73B902D7F3A03DE492743EB797FFF9613B94B39B89A9F69098697EC08AC81091A3FE0D2B341972557E3ABDDF13549399D38DA979452DC951F3A3A533170D2C9B7484B0F466B417CE71044AE3F5147765E600180DEAB016839AECC017A0C3D1B064D917C135ACE0CCD648D87C455188660D2FE7273359C18B91C80B93D99DB8926131A67A8E620BE937C2E876599CA0C41EC53A3E1B6BBFDACFA50439957EB06EBDED68E6E43DB4BBDF66F7F8ACDBC726E251717E308DA3571582E8BEA702CCBAC50C9D4E5C98E53DA38BB73424453EF7C4B8C90788AE18373F1712FDC75C1A4BC815FE3066C46AA297A10D3D03AEB268ED9B874494E5D85D37D18147723BD3869F273C08DD1CF7586AF446E10DB4E5464DEA8CEBCEFA386D9456ED6CB966C487ADD38C20A5CA82E905DDC2B72FF98156626B62A5116FE44B5259853E85E700F5EFFE4C11FEFA90AEEF42D22B74472A77FFFED8377FFAC2A39D4926FBFFC48556C49BB5FBB27121B9DFBD1876F3F7AE77745EE9F487BF9F0C3AF8ADC3FADCE7DF6EABDD3BF952DF999A4DDFF411C7CABC8FDB824F76BF74E5FF94B91FB09492FDFF9F4F43D4C3A12613E78FB0F0FBFF967995D22CD07F75F3FFDEBFD32BB4C9C1FBF7BF67629CE0D893CCF5EB97FF6E6C76576894091F889C6C824FACD9F4EBF2AB9BE2111E9A397DE78F4D68B6576894C1FBDF88FD3FBEF97D96542FDFD17DF7E8D354622D5D377FFF5E8375F9EBEF95251E2B18D1A9314BD4DAF3B559174BA9FB04E3F79E3DB7FBF7AF6C1E7671FBCAF3A6D3D7899534606F78FBF60CBC830FFEA7B679FBDF4F0E58FCE3EFA4475223BFDE2E5B34FBF7EF8D9370F3F7F47753AA31024037F4D006D87A16F39291878161AC67D816CC265CF1E685D2B292D4A9455E72A02923343D0414A5522365A95BDEE5D82C9923FD8B6B26785764068019BD5ED502FED7A8D2CCEF6312762DA79836CE68F98DA91A60D8344D505EE0EDA2AA331818AB36AB9E359CE0CB83ABCA388286AF709438AEAE82F97E02CB1B578910E7B54DAC1D824D946157553D29331707384215609C8AC73B6041E159EDA0C7C53AF731928146AE060AFBB01D204B4C2AE748757A1BCFA0655910F76359AA40ED934A430172E5DE44A2FAEF50CBE92FE748661890CFB03E46ADF4711B8141D214B7051CE4124B8D6D7D658B54FB5B28529014A3CE800904AEC516907E314B70480A42740358C081D5A1501D99236AB7891A5C1F46F00C802DE750E64017BFA0664A1AB673548E47E9F349471B763752D41F5BA6E6FD404B50E758666991CFBA62808FD7D95012670FE6D09CE82CB008B5807CCC099DBA145C0992BC7FECCCED54EAF227C297AC096F0621CCED5B1ACE63FDB9F9959A93F1D20594986FD9997250EA6127449BD4D1928634ED7DA5896BED7D237304B3AD41D9A2572EC0F9C25CEA4D5D62EB967290D30DCC759D7B6267FB1A7377056EB5067D635991CFB0367CCE15204269EF765099CC24F591D9D1C9F4D75E3D8E2B1C836BF03DCB14250A99472DDED006199131B2A13A112C559326A879BBFCA78693FF9088F79EF93DC0E61EE3E10E68E77348412E26318F1EFCE87C341E943274213834A9224312A7904A989528D5C004105ADF4C84F8950616D1253C38C711292C43E874790DA7A2A912B3760628AF8D6594294D2857934991D8C1A49FCA93F114D4C9554124FB92C88E5832FE812A2C5D4CA902ABE4828EC6E3FCB6B49EAEC2E295A3AEC703B83FB295394B0994130AED8272CB0324ACF8FD21399AEDB48D15B76C43393A4AEB707469B9DA0E8258964963A23390FC4097928F157E07551ECB1C0722E9BDCE46C13FB28284BA336BF848F918998A67472CEE9A5ECEC9CE92A3EA54B79283B2D6F939192476858466A9CDC569C42298C607A21AB60A3DA516DBB2358F2088E8C9195278E35CE1C4D30527454A881F6DA8C14BF5522E2A4DA91579D432F8697843A2465A7F498ABFDF12D7E2F45819B15272E75CE5CCC71937FCAA28CFC1ADCAC36DF7398A961EFAF61F1C7BACAAAD4159C54B3F1B7094B89F958CC492583731D9333CB4B7C2F2167A6CCC8DCBE3624BE0A2F5287D4EC9D752C9E4C6F894D94542392DA38DBE4267E5B99E59CC8B42633AE612D16ECF364D63433FACBFC466261CC29BE6D8EB2E0C1794212AD931B6578F32AAADEF12658D4E13C6530CE420EEF3C36D60FC83BCD688CAC9088CB4BB5B6A829F2033081D4D7E4A2AE0D7793973D2F8108EC83E432CB8E3D65B2714D5782ADF3BC4A81758A95E17C533D2F98FC5F4C7BE260A51CDB5F4E61177535C995F61A8A71C052182411A1810B02CE5D5DB492C6534F6CC71497668CDC3829A9055C4C9739A3C7E94A0FF0C574E7A1467172F334752A656C519C4E99AA4729B315D1947816A42A4A5874509C1496AC2BD522B4262BD3E2932ECD2C80264B304BD7A28607FFA408E29F7469725B58A4D79047F6B0005726BC109C12F4CE038252009E27ABD32A4382E2A4CA54754A6590509C5299CA52DA1C51D319736EC14C9DCC490F39252B4DD8947E507FBAAE549114266B49F976A6EAEC9573BC7C96A23129172F9613B37191AA4E897C921CA7467E51A748BC3B4E3009FFA04EAF7C7C1C2756A6AA53C25E17C74961C93A53148F5499AA3195A40F8413B3489AA24E81139E1127C7F95C8B761A88514038FD568B6A1A6F514035FDA683BC7958451276F3548DF195BDA74E0CAE2C4943AE79DC4442B2799A469F8847D7897E115F34FA46C43E24BA487CD1E456167790E15896AC31468B1086C4102D52B5E62148C428A4E622F22397EE4EFA56C931AD4F14D10A792B121DBA9043578898E4DA3FA19E200277D2545EF384CB0BA3792664A6A25E8AC844310C3974B2641D4277A1CDA173977EB849DA9EC338E0B5274DD6217410381C3A69AA0E9910443C6901114847149A94818C07B3C4EBC3D37526149E8EE82CAD8E989E7936561013C78FDADA21B770BF76F15988359C18E0F8C22C5CDAD8A15553910B8FED94E55E41A15FC26F6AC2591024A893A2FA80A83C2C538083A4FC6A93B8DA242EFF26D1C496C6FC66A437DA087B2CDE743EC27C33EB4E495524564B5407A8604EF8EB838276AED5C7849442BF2081077E21263E2C5D9D1A1DDB05A7487FD3A19A857021A96569CB0652CC75A2314A4B77EDDA30AD20D12F9C12714D88B343FC833ABD3276094EAC4C5D1A5CB14E244DF76C98C77EDD4D5B15897EE18A8CE741A9B5D897A5C1C3DC85A63E0A041414842F2C293C2AA578AA279B32CE054EA34C55A744C4BAC089111FD4E915212F705A45A2069D79480B82CE3C519D0E752F0EA726B932B7301CA75779EA8338B9F2A30F606EA97626AC885D5D22D1D222B4277AD4D900E0B9B756F4A02FFB3EE272567D506097B8F4B15155B81D88788C32E3696A31795C06D2B49326752C5AC6B792CE52D49EA714BF0BDFCADCAF9170B84CFB9DB84FA6FD0D731F4BDAD131CB321C20E61C3976E2E4383E0923385D4B32AC8D7FE9EEB8C9145F66B80A3CE7008651F602F7F0FC7AF28AFBB6EB80307375CD5D382FD2177A957C3A372E243E9DD09E8EE8E2FA9EA1099530B48940266C8C95EA47243B887AE224CC95C635D18C39C22875585DCC4DEB3DA4131C6F0D7F9596BD38D8FBC51DAAF8B9C1F50049FEE2607DF06B53D18B559B421517354529CE7DEEBB893540374064E1B599D1F08E40601D82E0075370FC43DDE6948E9B068861AE9B06A8518E9BC628667E915CFE2B12C1BD35CD34AB70D7ACDD2CCA4533A363A3811C5111D614413AF7CF141352026BE99ED9AC457B54002CC50669C6EFE4BF99D3D7F9373B0823014A859A5AD79F498B733103C8278FC50C10248EC5327AFB8EFE145B1E881968137624A62E0BB579A31DBAD99199819E733C2ACD524DDD29CD924C7D298D2071EE4E591F86F9C16385742FE80B3777AD34D145E230D30041D2B1D204C1D2B5D2C4402EBC2BCDCC54A467657D9894EE8F2674B8C209D214B1CC13D214B5D41FD258D352A74853D452D74853C452074903C470EFC7DACA251D7EB425ED8B7120ECADEAB5445BDFCC815175BBA9293251988195DC7A62B2680281AAE7CCFB0A80D59E69B5675AE89EA90D9DDFB8A2DEB63E50EDB6D7DBC965B5BA6860A0D24D6F0581E610C01D040DCC09B477A01192998BA036295D90099CEC56286B8E32C2BBCF00264A07BFF65051ED22B74245735490AE79C637A75C373735B9B12E5672B9EDB52BB7D25FAEFEA131E126579F4CE121676024175E720D8F1829FF383D501385DB50631887B5DE4E1F51CD69976392F2CC1884C5DB1041DF93ABE5719458908DAC132287B3DE8AD893EF8CB569E6EE6CAA860511F385B1CF555E7316BD98BEA08033656B9847ABB1F674139357F632940A6E073A7143E42F07B2554AAF0774186849F678FDD2620D3B64E134254DFE6E20ACF22D8BE50697D223FF4B8E30ECE578A63D6D07665E10D6AA1ED15856C01908636F2C5EFDE29741D9DB174BB10C2E471C5803E1E6171E577EF13355E780AB33532D13E0B4C3C22FD7DAC8061A6386804E00EF7EAC8E0A2F782CF7F2583378FBB24C73CB00BACE27BA5AA05B8E998E3AF3ECE544479FDB368AEDDE8F694EFE26CDB2CE72F4F9679F1187C5C66421A71183BD5798AB7A61665941471FAFF61274CC1971B348E9FD009DC2F333CB0A3A85B0E20DAC102D81AC88B684B54439BC792D48D5314B980924DEABF0E16CA8265A84F9430BF3134F1A3EE521970860D99B085B437B3FB9E99D9D97496367D1F5523323532BF59D57279145B9C6F43841545DFAB1A22E5EFC5E59FC7195F0E3E22A2BA280D2F552FB38A656EA3BAF4E496C477E8DF8464D50299E455C6F45CC4A59B4739560E7BC8A99F71F15EBC5B43651C558968A9AC5F110A581D395E2A68B9155113CB04E78755E45F94739FD34CA3A433B4DE5D1CD62B54B68E261D719D2F8475E0D644877AAA6258ADA2E0EDAC859832A140A8C0AF38D560B96340E7B2386B08B031D3FC0141B4407A9C6C3AB9B6008B388715FD76FCE9AC5044C1787355EA2C1B39810E83559B308D47412D4DCC49012694BFCF7C05B604FDB51CA9B61A76BF6549BB40D861D37811D81BACB7D36DC186BBA08246E90398C4ECE7FADDAD442DE616470134B39BB6FE03FBADC9C3D06427D375862F9662AACBC2050B899E8DEEC4B939BA39B4840CE34F319DEBC0443675292D84434BD6C4A2B89CEF324DB8FB9A5896AD13C0B730B22023688C076F2800BB022F4D94ADEAD49DED5790EB831CA7279BA0FED3DEF7A1CCDE20875194EF75D22524B62AAAAAA3F0D614EB679F3FA2CDD3D9BE8026AA693B8CB5FF79E8E1DD72EDABDCBF1C81690486C60B9EF7922CB28F1419F9C1494AE314153448472F615A6BB5B703A7311B1F0BA370647B04EDB6E87F00A9C00EBE446FE60A898885C1024DB372F396012806998D328CBA39F08C3F6F4F8C9FF03A310A82B6DF70000 , N'6.1.3-40302')

