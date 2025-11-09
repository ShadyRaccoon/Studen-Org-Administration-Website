/*
-- StudentOrg Population Script
-- Insert sample data into all tables

USE StudentOrg;

###########################################################
-- POPULATE MEMBERS (30 members)
###########################################################

INSERT INTO members (first_name, last_name, faculty, join_date, leave_date) VALUES
('Alexandru', 'Popescu', 'Faculty of Architecture', '2022-09-01', NULL),
('Maria', 'Ionescu', 'Faculty of Urban Planning', '2022-09-15', NULL),
('Cristian', 'Gheorghe', 'Faculty of Architecture', '2022-10-01', NULL),
('Elena', 'Vasile', 'Faculty of Urban Planning', '2022-10-05', NULL),
('Andrei', 'Mihai', 'Faculty of Architecture', '2023-01-10', NULL),
('Laura', 'Dumitrescu', 'Faculty of Urban Planning', '2023-02-15', NULL),
('Bogdan', 'Popa', 'Faculty of Architecture', '2023-03-01', NULL),
('Ioana', 'Stoica', 'Faculty of Urban Planning', '2023-03-20', NULL),
('Daniel', 'Radu', 'Faculty of Architecture', '2023-04-05', NULL),
('Silvia', 'Marin', 'Faculty of Urban Planning', '2023-04-15', NULL),
('Florin', 'Stanescu', 'Faculty of Architecture', '2023-05-01', NULL),
('Carmen', 'Enescu', 'Faculty of Urban Planning', '2023-05-10', NULL),
('Mihai', 'Barbu', 'Faculty of Architecture', '2023-06-01', NULL),
('Roxana', 'Seban', 'Faculty of Urban Planning', '2023-06-15', NULL),
('Sergiu', 'Sandu', 'Faculty of Architecture', '2023-07-01', NULL),
('Adriana', 'Neagu', 'Faculty of Urban Planning', '2023-07-20', NULL),
('Constantin', 'Isac', 'Faculty of Architecture', '2023-08-01', NULL),
('Valentina', 'Mutu', 'Faculty of Urban Planning', '2023-08-15', NULL),
('Vlad', 'Iacob', 'Faculty of Architecture', '2023-09-01', NULL),
('Patricia', 'Roman', 'Faculty of Urban Planning', '2023-09-10', NULL),
('Radu', 'Pavel', 'Faculty of Architecture', '2024-01-15', NULL),
('Mihaela', 'Georgescu', 'Faculty of Urban Planning', '2024-02-01', NULL),
('Ionut', 'Nedelcu', 'Faculty of Architecture', '2024-02-20', NULL),
('Tatiana', 'Ciocan', 'Faculty of Urban Planning', '2024-03-01', NULL),
('Costel', 'Marinescu', 'Faculty of Architecture', '2024-03-15', NULL),
('Diana', 'Postolache', 'Faculty of Urban Planning', '2024-04-01', NULL),
('Theodor', 'Milosevici', 'Faculty of Architecture', '2024-04-20', NULL),
('Simona', 'Teme', 'Faculty of Urban Planning', '2024-05-01', NULL),
('Gabriel', 'Vlad', 'Faculty of Architecture', '2024-05-15', NULL),
('Juliana', 'Stefanescu', 'Faculty of Urban Planning', '2024-06-01', NULL);

###########################################################
-- POPULATE DEPARTMENTS (6 departments)
###########################################################

INSERT INTO departments (department_name, department_alias) VALUES
('Human Resources', 'HR'),
('Public Relations', 'PR'),
('Fundraising', 'FR'),
('Image & Media', 'IMG'),
('Representation', 'REPRE'),
('Socio-Cultural', 'SOCIO');

###########################################################
-- POPULATE DEPARTMENT_MEMBERS
###########################################################

INSERT INTO department_members (member_id, department_id) VALUES
-- HR Department
(1, 1), (2, 1), (3, 1),
-- PR Department
(4, 2), (5, 2), (6, 2),
-- Fundraising Department
(7, 3), (8, 3), (9, 3),
-- Image & Media Department
(10, 4), (11, 4), (12, 4),
-- Representation Department
(13, 5), (14, 5), (15, 5),
-- Socio-Cultural Department
(16, 6), (17, 6), (18, 6),
-- Multiple department members
(19, 1), (19, 2),
(20, 3), (20, 4),
(21, 2), (21, 5),
(22, 4), (22, 6),
(23, 1), (23, 3),
(24, 5), (24, 6),
(25, 2), (25, 4),
(26, 1), (26, 6),
(27, 3), (27, 5),
(28, 2), (28, 6),
(29, 4), (29, 1),
(30, 5), (30, 3);

###########################################################
-- POPULATE BUREAU_POSITIONS (9 positions)
###########################################################

INSERT INTO bureau_positions (position_name, position_alias) VALUES
('President', 'P'),
('Vice-President', 'VP'),
('General Secretary', 'SG'),
('HR Coordinator', 'HR'),
('PR Coordinator', 'PR'),
('Fundraising Coordinator', 'FR'),
('Image Coordinator', 'IMG'),
('Representation Coordinator', 'REPRE'),
('Socio-Cultural Coordinator', 'SOCIO');

###########################################################
-- POPULATE BUREAU_MEMBERS
###########################################################

INSERT INTO bureau_members (member_id, position_id, start_term_date, end_term_date) VALUES
-- Current bureau (2024-2025 term)
(1, 1, '2024-09-01', NULL),  -- Alexandru - President
(2, 2, '2024-09-01', NULL),  -- Maria - Vice-President
(3, 3, '2024-09-01', NULL),  -- Cristian - General Secretary
(4, 4, '2024-09-01', NULL),  -- Elena - HR Coordinator
(5, 5, '2024-09-01', NULL),  -- Andrei - PR Coordinator
(6, 6, '2024-09-01', NULL),  -- Laura - FR Coordinator
(7, 7, '2024-09-01', NULL),  -- Bogdan - IMG Coordinator
(8, 8, '2024-09-01', NULL),  -- Ioana - REPRE Coordinator
(9, 9, '2024-09-01', NULL),  -- Daniel - SOCIO Coordinator
-- Previous bureau (2023-2024 term - expired)
(10, 1, '2023-09-01', '2024-08-31'),
(11, 2, '2023-09-01', '2024-08-31'),
(12, 3, '2023-09-01', '2024-08-31');

###########################################################
-- POPULATE PICTURES
###########################################################

INSERT INTO pictures (location) VALUES
('https://drive.google.com/uc?id=1kK9Z8xY7pQrStUvWxYzAbCdEfGhIjKlMnOpQrStUv'),
('https://drive.google.com/uc?id=2lL0aZ9yRsStUvWxYzAbCdEfGhIjKlMnOpQrStUw'),
('https://drive.google.com/uc?id=3mM1bA0zRtStUvWxYzAbCdEfGhIjKlMnOpQrStUx'),
('https://drive.google.com/uc?id=4nN2cB1aRuStUvWxYzAbCdEfGhIjKlMnOpQrStUy'),
('https://drive.google.com/uc?id=5oO3dC2bRvStUvWxYzAbCdEfGhIjKlMnOpQrStUz');

###########################################################
-- POPULATE POSTS
###########################################################

INSERT INTO posts (post_author, post_banner, post_description, post_content) VALUES
(
    'Alexandru Popescu',
    'https://drive.google.com/uc?id=1kK9Z8xY7pQrStUvWxYzAbCdEfGhIjKlMnOpQrStUv',
    'Join us for our monthly networking event',
    'We are excited to announce our September networking event! Come meet fellow architects and urbanists. Light refreshments will be served. RSVP by September 10th.'
),
(
    'Maria Ionescu',
    'https://drive.google.com/uc?id=2lL0aZ9yRsStUvWxYzAbCdEfGhIjKlMnOpQrStUw',
    'New project opportunity: Urban redesign competition',
    'Our organization is hosting an urban redesign competition for local neighborhoods. Interested? Submit your proposals by October 1st. Winners will receive recognition and potential implementation support.'
),
(
    'Cristian Gheorghe',
    'https://drive.google.com/uc?id=3mM1bA0zRtStUvWxYzAbCdEfGhIjKlMnOpQrStUx',
    'Success story: Spring semester initiatives',
    'This spring we successfully organized 5 workshops, hosted 2 guest speakers, and completed 3 urban analysis projects. Thank you all for your dedication!'
),
(
    'Elena Vasile',
    'https://drive.google.com/uc?id=4nN2cB1aRuStUvWxYzAbCdEfGhIjKlMnOpQrStUy',
    'Call for volunteers: Summer campaign',
    'We need volunteers for our summer urban awareness campaign. Tasks include community outreach, data collection, and event coordination. All skill levels welcome!'
),
(
    'Andrei Mihai',
    'https://drive.google.com/uc?id=5oO3dC2bRvStUvWxYzAbCdEfGhIjKlMnOpQrStUz',
    'Workshop announcement: Sustainable urban design',
    'Learn about sustainable urban design practices from industry experts. Free workshop for all members. Saturday, October 15th at 10am. Register in the office.'
);

###########################################################
-- POPULATE POSTS_PICTURES
###########################################################

INSERT INTO posts_pictures (picture_id, post_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

###########################################################
-- POPULATE ACCOUNTS
###########################################################

INSERT INTO accounts (is_active, member_id, account_password, username, email, phone, creation_date, termination_date) VALUES
(1, 1, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'apopescu', 'alexandru.popescu@uni.ro', '+40721234567', '2024-08-15', NULL),
(1, 2, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'mionescu', 'maria.ionescu@uni.ro', '+40722234567', '2024-08-16', NULL),
(1, 3, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'cgheorghe', 'cristian.gheorghe@uni.ro', '+40723234567', '2024-08-17', NULL),
(1, 4, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'evasile', 'elena.vasile@uni.ro', '+40724234567', '2024-08-18', NULL),
(1, 5, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'amihai', 'andrei.mihai@uni.ro', '+40725234567', '2024-08-19', NULL),
(1, 6, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'ldumitrescu', 'laura.dumitrescu@uni.ro', '+40726234567', '2024-08-20', NULL),
(1, 7, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'bpopa', 'bogdan.popa@uni.ro', '+40727234567', '2024-08-21', NULL),
(1, 8, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'istoica', 'ioana.stoica@uni.ro', '+40728234567', '2024-08-22', NULL),
(0, 9, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'dradu', 'daniel.radu@uni.ro', '+40729234567', '2024-08-23', '2024-09-01'),
(1, 10, '$2a$11$R9h7cIPz0gi.URNNGH3.e.5MeINb3v6P.qRJBZJOZbXLt1JdKqLQW', 'smarin', 'silvia.marin@uni.ro', '+40730234567', '2024-08-24', NULL);

###########################################################
-- POPULATE ACCOUNT_REQUESTS
###########################################################

INSERT INTO account_requests (request_date, request_status, request_author, request_author_role, request_description, requested_first_name, requested_last_name) VALUES
('2024-09-05', 'accepted', 'Alexandru Popescu', 'President', 'New member joining HR department', 'Florin', 'Stanescu'),
('2024-09-08', 'pending', 'Maria Ionescu', 'Vice-President', 'New volunteer for PR initiatives', 'Carmen', 'Enescu'),
('2024-09-10', 'denied', 'Cristian Gheorghe', 'General Secretary', 'External collaborator for project', 'Mihai', 'Barbu'),
('2024-09-12', 'accepted', 'Elena Vasile', 'HR Coordinator', 'New member from urban planning', 'Roxana', 'Seban'),
('2024-09-15', 'pending', 'Andrei Mihai', 'PR Coordinator', 'Designer for marketing materials', 'Sergiu', 'Sandu');
*/