-- DOCUMENTATION HERE !
-- IT IS BETTER TO ADD DOCUMENTATION IN THE CODE ITSELF FOR OTHER DEVS ! :)  
create table [Zip_Code] (
  zip int not null,
  city varchar(50) not null,

  primary key (zip)
);

create table [Address] (
  address_id int not null identity,
  street varchar(100) not null,
  additional_info varchar(50),
  zip int not null,

  primary key (address_id),
  constraint FK_address_zip foreign key (zip)
    references [Zip_Code] (zip)
    on update cascade
);

create table [Member] (
  member_id int not null identity,
  ssn varchar(10) not null,
  f_name varchar(25) not null,
  l_name varchar(25) not null,
  home_address_id int not null,
  campus_address_id int not null,

  primary key (member_id),
  constraint FK_member_home_address_id foreign key (home_address_id)
    references [Address] (address_id)
    on update cascade,
  constraint FK_member_campus_address_id foreign key (campus_address_id)
    references [Address] (address_id)
    on update cascade
);

create table [Member_Type] (
  [type_id] int not null identity,
  [type] varchar(25) not null unique,    
  
  primary key ([type_id])
);

create table [Member_Type_Assignment] (
  [member_id] int not null,
  [type_id] int not null,

    primary key ([member_id],[type_id]),
    constraint FK_member_type_assignment_member_id foreign key ([member_id])
        references [Member] ([member_id])
            on update cascade,
        constraint FK_member_type_assignment_type_id foreign key ([type_id])
    references [Member_Type] ([type_id])
        on update cascade
    );


create table [Phone_Number] (
  member_id int not null,
  phone_number varchar(20) not null,
  primary key (member_id, phone_number),
  
  constraint FK_phone_number_member_id foreign key (member_id)
    references [Member] (member_id)
    on update cascade
    on delete cascade
);

create table [Card] (
  member_id int not null,
  photo_path varchar(50) not null,
  primary key (member_id, photo_path),
  
  constraint FK_card_member_id foreign key (member_id)
    references [Member] (member_id)
    on update cascade
    on delete cascade
);

create table [Staff] (
  member_id int not null,
  job_title varchar(50) not null,
  primary key (member_id, job_title),
  
  constraint FK_staff_member_id foreign key (member_id)
    references [Member] (member_id)
    on update cascade
    on delete cascade
);

create table [Author] (
  author_id int not null identity,
  first_name varchar(50) not null,
  last_name varchar(50) not null,

  primary key (author_id)
);



create table [Material] (
  material_id int not null identity,
  isbn varchar(100) not null,
  title varchar(100) not null,
  [language] varchar(50) not null,
  lendable bit not null,
  [type_id] int not null,
  [description] varchar(max) not null,

  primary key (material_id),
    constraint FK_material_type_id foreign key ([type_id])
        references [Material_Type] ([type_id])
          on update cascade
);

create table [Material_Type] (
    [type_id] int not null identity,
    [type] varchar(50) not null,

    primary key ([type_id])
);

create table [Material_Subject] (
  material_id int not null,
  [subject] varchar(50) not null,

  primary key (material_id, subject),  
  constraint FK_subject_material_id foreign key (material_id)
    references [Material] (material_id)
    on update cascade
    on delete cascade
);

create table [Material_Subject_Assignment] (
    [material_id] int not null,
    [subject_id] int not null,

    primary key ([material_id], [subject_id]),
    constraint FK_material_subject_assignment_material_id foreign key ([material_id])
        references [Material] ([material_id])
        on update cascade
    on delete cascade,
    constraint FK_material_subject_assignment_subject_id foreign key ([subject_id])
        references [Material_Subject] ([subject_id])
        on update cascade
);

create table [Material_Author] (
  material_id int not null,
  author_id int not null,
  primary key (material_id, author_id),
  
  constraint FK_material_authors_material_id foreign key (material_id)
    references [Material] (material_id)
    on update cascade
    on delete cascade,
  constraint FK_material_authors_author_id foreign key (author_id)
    references [Author] (author_id)
    on update cascade
    on delete no action
);

create table [Volume] (
  volume_id int not null identity,
  material_id int not null,
  home_location_id int not null,
  current_location_id int not null,
  
  primary key (volume_id),
  constraint FK_volume_material_id foreign key (material_id)
    references [Material] (material_id)
    on update no action
    on delete no action,
  constraint FK_volume_home_location_id foreign key (home_location_id)
    references [Address] (address_id)
    on update no action
    on delete no action,
  constraint FK_volume_current_location_id foreign key (current_location_id)
    references [Address] (address_id)
    on update no action
    on delete no action
);

create table [Loan] (
  loan_id int not null identity,
  member_id int not null,
  volume_id int not null,
  loan_date date not null,
  due_date date not null,
  extensions int not null,
  returned_date date,
  
  primary key (loan_id),
  constraint FK_loan_member_id foreign key (member_id)
    references [Member] (member_id)
    on update no action
    on delete no action,
  constraint FK_loan_volume_id foreign key (volume_id)
    references [Volume] (volume_id)
    on update no action
    on delete no action
);

CREATE TABLE [Aquire_Reason](
    reason_id int not null identity,
    reason varchar(42),
    primary key (reason_id),
);

CREATE TABLE [Acquire] (
  material_id int not null,
  additional_info varchar(100) not null,
  reason_id int not null,

  PRIMARY KEY (material_id),
  CONSTRAINT FK_acquire_material_id foreign key (material_id)
    references [Material] (material_id)
    on update cascade
    on delete no action,
       CONSTRAINT FK_aquire_reason_id foreign key (reason_id)
        references [Aquire_Reason] (reason_id)
        on update cascade


);



--GO

--[dummy data]