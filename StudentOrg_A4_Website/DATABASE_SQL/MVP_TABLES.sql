/*
CREATE DATABASE StudentOrg;

USE StudentOrg;

###########################################################
		## TABELE PENTRU ORGANIZATIE 
###########################################################

CREATE TABLE members (
	member_id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR (100) NOT NULL,
    last_name VARCHAR (100) NOT NULL,
    faculty VARCHAR (150) NOT NULL,
    join_date DATE NOT NULL,
    leave_date DATE NULL
);

		## HR, PR, FR, IMG, REPRE, SOCIO
CREATE TABLE departments (
	department_id INT PRIMARY KEY AUTO_INCREMENT,
    department_name VARCHAR(100) NOT NULL,
    department_alias VARCHAR(10) NOT NULL,
    CONSTRAINT unique_name UNIQUE(department_name),
    CONSTRAINT unique_alias UNIQUE(department_alias)
);

CREATE TABLE department_members (
	member_id INT,
    department_id INT,
    FOREIGN KEY (member_id) REFERENCES members(member_id),
    FOREIGN KEY (department_id) REFERENCES departments(department_id),
    PRIMARY KEY(member_id, department_id)
);

		## PREȘEDINTE, VICE-PREȘEDINTE, SECRETAR GENERAL
        ## COORDONATOR
			## RESURSE UMANE
            ## PUBLIC RELATIONS
            ## FUNDRAISING
            ## IMAGINE
            ## REPREZENTARE
            ## SOCIO-CULTURAL
		## ALIAS:
			# P, VP, SG, HR, PR, FR, IMG, REPRE, SOCIO
CREATE TABLE bureau_positions (
	position_id INT PRIMARY KEY AUTO_INCREMENT,
    position_name VARCHAR (50) NOT NULL,
    position_alias VARCHAR (30) NOT NULL,
    CONSTRAINT unique_name UNIQUE(position_name),
    CONSTRAINT unique_alias UNIQUE(position_alias)
);

CREATE TABLE bureau_members (
	member_id INT,
	position_id INT,
    start_term_date DATE NOT NULL,
    end_term_date DATE NULL,
    FOREIGN KEY (member_id) REFERENCES members(member_id),
    FOREIGN KEY (position_id) REFERENCES bureau_positions(position_id)
);

###########################################################
		## TABELE PENTRU SITE ORGANIZATIE 
###########################################################

CREATE TABLE accounts (
	account_id INT PRIMARY KEY AUTO_INCREMENT,
    is_active BOOL NOT NULL,
    member_id INT,
    account_password VARCHAR(256) NOT NULL, #asp core technical reasons
    username VARCHAR(200) NOT NULL, ## only for login
    email VARCHAR(254) NOT NULL, ## 254 max email address length
    phone VARCHAR(25) NOT NULL, ## in case of international students with dif no. formats
    creation_date DATE NOT NULL,
    termination_date DATE NULL,
    FOREIGN KEY (member_id) REFERENCES members(member_id),
    CONSTRAINT unique_username UNIQUE (username),
    CONSTRAINT unique_email UNIQUE (email),
    CONSTRAINT unique_phone UNIQUE (phone)
);

CREATE TABLE pictures (
	picture_id INT PRIMARY KEY AUTO_INCREMENT,
    location VARCHAR(500) NOT NULL ## am obs ca linkurile la pozele mele au undeva la 80-90 de caractere, dar sa fiu safe
);

CREATE TABLE posts (
	post_id INT PRIMARY KEY AUTO_INCREMENT,
    post_author VARCHAR(200) NOT NULL, ## numele persoanei care a facut postarea, nu username
    post_banner VARCHAR(500) NOT NULL,
    post_description VARCHAR(500) NOT NULL,
    post_content TEXT NOT NULL
);

CREATE TABLE posts_pictures (
	picture_id INT,
    post_id INT,
    FOREIGN KEY (picture_id) REFERENCES pictures(picture_id),
    FOREIGN KEY (post_id) REFERENCES posts(post_id),
    PRIMARY KEY (picture_id, post_id)
);

###########################################################
		## TABELE PENTRU CERERE CREARE CONT LA ADMIN
###########################################################

CREATE TABLE account_requests (
	request_id INT PRIMARY KEY AUTO_INCREMENT,
    request_date DATE  NOT NULL,
    request_status VARCHAR (10) NOT NULL, ## pending/denied/accepted
    request_author VARCHAR (200) NOT NULL, ## numele autorului, nu username
    request_author_role VARCHAR(50) NOT NULL, ## numele pozitiei autorului (doar membrii din biroul de conducere pot face cereri pentru noi conturi)
    request_description VARCHAR(500),
    
    requested_first_name VARCHAR(100) NOT NULL,
    requested_last_name VARCHAR(100) NOT NULL,
    
    CONSTRAINT valid_status CHECK (request_status IN ('pending', 'denied', 'accepted'))
    ## se cauta dupa nume si prenume membrul pentru care se cere generarea contului
    ## i se trimit pe mail username si parola generate automat
);
*/