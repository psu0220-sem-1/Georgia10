create table zips (
  zip int not null primary key,
  city varchar(50) not null
);

create table addresses (
  address_id int not null primary key identity,
  street varchar(50) not null,
  additional_info varchar(50) not null,
  zip int not null,

  constraint FK_address_zip foreign key (zip)
    references zips (zip)
    on update cascade on delete no action
);

create table members (
  member_id int not null primary key identity,
  ssn varchar(10) not null,
  f_name varchar(25) not null,
  l_name varchar(25) not null,
  home_address_id int not null,
  campus_address_id int not null,

  constraint FK_member_home_address_id foreign key (home_address_id)
    references addresses (address_id)
    on delete no action,
  constraint FK_member_campus_address_id foreign key (campus_address_id)
    references addresses (address_id)
    on delete no action
);

create table member_types (
  member_id int not null,
  type varchar(9) not null check (type in('student', 'staff', 'professor')),                                      
  primary key (member_id, type),
                                          
  constraint FK_type_member_id foreign key (member_id)
    references members (member_id)
    on update cascade on delete cascade
);

create table phone_numbers (
  member_id int not null,
  phone_number varchar(20) not null,
  primary key (member_id, phone_number),
  
  constraint FK_phone_number_member_id foreign key (member_id)
    references members (member_id)
    on update cascade on delete cascade
);

create table cards (
  member_id int not null,
  photo_path varchar(50) not null,
  primary key (member_id, photo_path),
  
  constraint FK_card_member_id foreign key (member_id)
    references members (member_id)
    on update cascade on delete cascade
);

create table staff (
  member_id int not null,
  job_title varchar(50) not null,
  primary key (member_id, job_title),
  
  constraint FK_staff_member_id foreign key (member_id)
    references members (member_id)
    on update cascade on delete cascade
);

create table authors (
  author_id int not null primary key identity,
  first_name varchar(50) not null,
  last_name varchar(50) not null
);

create table materials (
  material_id int not null primary key identity,
  isbn varchar(100) not null,
  title varchar(100) not null,
  language varchar(50) not null,
  lendable bit not null,
  type varchar(14) not null check (type in ('reference book', 'maps', 'rare book', 'book')),
  description varchar(max) not null,
);

create table material_subjects (
  material_id int not null,
  subject varchar(50) not null,
  primary key (material_id, subject),
  
  constraint FK_subject_material_id foreign key (material_id)
    references materials (material_id)
    on update cascade on delete cascade
);

create table material_authors (
  material_id int not null,
  author_id int not null,
  primary key (material_id, author_id),
  
  constraint FK_material_authors_material_id foreign key (material_id)
    references materials (material_id)
    on update cascade on delete cascade,
  constraint FK_material_authors_author_id foreign key (author_id)
    references authors (author_id)
    on update cascade on delete no action
);

create table volumes (
  volume_id int not null primary key identity,
  material_id int not null,
  home_location_id int not null,
  current_location_id int not null,
  
  constraint FK_volume_material_id foreign key (material_id)
    references materials (material_id)
    on update no action on delete no action,
  constraint FK_volume_home_location_id foreign key (home_location_id)
    references addresses (address_id)
    on update no action on delete no action,
  constraint FK_volume_current_location_id foreign key (current_location_id)
    references addresses (address_id)
    on update no action on delete no action
);

create table loans (
  loan_id int not null primary key identity,
  member_id int not null,
  volume_id int not null,
  loan_date date not null,
  due_date date not null,
  extensions int not null,
  returned_date date,
  
  constraint FK_loan_member_id foreign key (member_id)
    references members (member_id)
    on update no action on delete no action,
  constraint FK_loan_volume_id foreign key (volume_id)
    references volumes (volume_id)
    on update no action on delete no action
);

create table acquire (
  material_id int not null primary key,
  additional_info varchar(100) not null,
  reason_for_acquiring varchar(11) not null check (reason_for_acquiring in ('rare', 'destroyed', 'lost', 'unspecified')),

  constraint FK_acquire_material_id foreign key (material_id)
    references materials (material_id)
    on update no action on delete no action
);
