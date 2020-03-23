-- DOCUMENTATION HERE !
-- IT IS BETTER TO ADD DOCUMENTATION IN THE CODE ITSELF FOR OTHER DEVS ! :)  
CREATE TABLE [Zip_Code] (
    [zip] INT not null,
    [city] VARCHAR(50) not null,

    PRIMARY KEY ([zip])
);

CREATE TABLE [Address] (
    [address_id] INT not null IDENTITY,
    [street] VARCHAR(100) not null,
    [additional_info] VARCHAR(50),
    [zip] INT not null,

    PRIMARY KEY ([address_id]),
    CONSTRAINT FK_address_zip FOREIGN KEY ([zip])
        REFERENCES [Zip_Code] ([zip])
        ON UPDATE CASCADE
);

CREATE TABLE [Member] (
    [member_id] INT not null IDENTITY,
    [ssn] VARCHAR(10) not null,
    [f_name] VARCHAR(25) not null,
    [l_name] VARCHAR(25) not null,
    [home_address_id] INT not null,
    [campus_address_id] INT not null,

    PRIMARY KEY ([member_id]),
    CONSTRAINT [FK_member_home_address_id] FOREIGN KEY ([home_address_id])
        REFERENCES [Address] (address_id),
        -- ON UPDATE CASCADE,
    CONSTRAINT [FK_member_campus_address_id] FOREIGN KEY ([campus_address_id])
        REFERENCES [Address] ([address_id])
        -- ON UPDATE CASCADE
);

CREATE TABLE [Member_Type] (
    [type_id] INT not null IDENTITY,
    [type] VARCHAR(25) not null unique,    
  
    PRIMARY KEY ([type_id])
);

CREATE TABLE [Member_Type_Assignment] (
    [member_id] INT not null,
    [type_id] INT not null,

    PRIMARY KEY ([member_id],[type_id]),
    CONSTRAINT [FK_member_type_assignment_member_id] FOREIGN KEY ([member_id])
        REFERENCES [Member] ([member_id])
        ON UPDATE CASCADE,
    CONSTRAINT [FK_member_type_assignment_type_id] FOREIGN KEY ([type_id])
        REFERENCES [Member_Type] ([type_id])
        ON UPDATE CASCADE
    );


CREATE TABLE [Phone_Number] (
    [member_id] INT not null,
    [phone_number] VARCHAR(20) not null,
    PRIMARY KEY (member_id, phone_number),
  
    CONSTRAINT [FK_phone_number_member_id] FOREIGN KEY ([member_id])
        REFERENCES [Member] ([member_id])
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE [Card] (
    [member_id] INT not null,
    [photo_path] VARCHAR(50) not null,
    PRIMARY KEY ([member_id], [photo_path]),
  
    CONSTRAINT [FK_card_member_id] FOREIGN KEY ([member_id])
        REFERENCES [Member] ([member_id])
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE [Staff] (
    [member_id] INT not null,
    [job_title] VARCHAR(50) not null,
    PRIMARY KEY ([member_id], [job_title]),
  
    CONSTRAINT [FK_staff_member_id] FOREIGN KEY ([member_id])
        REFERENCES [Member] ([member_id])
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE [Author] (
    [author_id] INT not null IDENTITY,
    [first_name] VARCHAR(50) not null,
    [last_name] VARCHAR(50) not null,

    PRIMARY KEY ([author_id])
);



CREATE TABLE [Material] (
    [material_id] INT not null IDENTITY,
    [isbn] VARCHAR(100) not null,
    [title] VARCHAR(100) not null,
    [language] VARCHAR(50) not null,
    [lendable] BIT not null,
    [type_id] INT not null,
    [description] VARCHAR(MAX) not null,

    PRIMARY KEY ([material_id]),
    CONSTRAINT [FK_material_type_id] FOREIGN KEY ([type_id])
        REFERENCES [Material_Type] ([type_id])
        ON UPDATE CASCADE
);

CREATE TABLE [Material_Type] (
    [type_id] INT not null IDENTITY,
    [type] VARCHAR(50) not null,

    PRIMARY KEY ([type_id])
);

CREATE TABLE [Material_Subject] (
    [subject_id] INT not null IDENTITY,
    [subject] VARCHAR(50) not null,

    PRIMARY KEY (subject_id),  
);

CREATE TABLE [Material_Subject_Assignment] (
    [material_id] INT not null,
    [subject_id] INT not null,

    PRIMARY KEY ([material_id], [subject_id]),
    CONSTRAINT [FK_material_subject_assignment_material_id] FOREIGN KEY ([material_id])
        REFERENCES [Material] ([material_id])
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT [FK_material_subject_assignment_subject_id] FOREIGN KEY ([subject_id])
        REFERENCES [Material_Subject] ([subject_id])
        ON UPDATE CASCADE
);

CREATE TABLE [Material_Author] (
    [material_id] INT not null,
    [author_id] INT not null,
    PRIMARY KEY ([material_id], [author_id]),
  
    CONSTRAINT [FK_material_authors_material_id] FOREIGN KEY ([material_id])
        REFERENCES [Material] (material_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT [FK_material_authors_author_id] FOREIGN KEY ([author_id])
        REFERENCES [Author] ([author_id])
        ON UPDATE CASCADE
);

CREATE TABLE [Volume] (
    [volume_id] INT not null IDENTITY,
    [material_id] INT not null,
    [home_location_id] INT not null,
    [current_location_id] INT not null,
  
    PRIMARY KEY ([volume_id]),
    CONSTRAINT [FK_volume_material_id] FOREIGN KEY ([material_id])
        REFERENCES [Material] ([material_id])
        ON UPDATE NO ACTION,
    CONSTRAINT [FK_volume_home_location_id] FOREIGN KEY ([home_location_id])
        REFERENCES [Address] ([address_id])
        ON UPDATE NO ACTION,
    CONSTRAINT [FK_volume_current_location_id] FOREIGN KEY ([current_location_id])
        REFERENCES [Address] ([address_id])
        ON UPDATE NO ACTION
);

CREATE TABLE [Loan] (
    [loan_id] INT not null IDENTITY,
    [member_id] INT not null,
    [volume_id] INT not null,
    [loan_date] DATE not null,
    [due_date] DATE not null,
    [extensions] INT not null,
    [returned_date] DATE,
  
    PRIMARY KEY ([loan_id]),
    CONSTRAINT [FK_loan_member_id] FOREIGN KEY ([member_id])
        REFERENCES [Member] ([member_id])
        ON UPDATE NO ACTION,
    CONSTRAINT [FK_loan_volume_id] FOREIGN KEY ([volume_id])
        REFERENCES [Volume] ([volume_id])
        ON UPDATE NO ACTION
);

CREATE TABLE [Acquire_Reason](
    [reason_id] INT not null IDENTITY,
    [reason] VARCHAR(42),

    PRIMARY KEY ([reason_id]),
);

CREATE TABLE [Acquire] (
    [material_id] INT not null,
    [additional_info] VARCHAR(100) not null,
    [reason_id] INT not null,

    PRIMARY KEY (material_id),
    CONSTRAINT [FK_acquire_material_id] FOREIGN KEY ([material_id])
        REFERENCES [Material] ([material_id])
        ON UPDATE CASCADE,
    CONSTRAINT [FK_acquire_reason_id] FOREIGN KEY ([reason_id])
        REFERENCES [Acquire_Reason] (reason_id)
        ON UPDATE CASCADE
);



--GO

--[dummy data]