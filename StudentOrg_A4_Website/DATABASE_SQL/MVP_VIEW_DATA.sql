/*
-- StudentOrg Verification Queries
-- Run these to verify all data was inserted correctly

USE StudentOrg;

###########################################################
-- VIEW ALL MEMBERS
###########################################################
SELECT 'MEMBERS' AS table_name, COUNT(*) AS row_count FROM members;
SELECT * FROM members;

###########################################################
-- VIEW ALL DEPARTMENTS
###########################################################
SELECT 'DEPARTMENTS' AS table_name, COUNT(*) AS row_count FROM departments;
SELECT * FROM departments;

###########################################################
-- VIEW ALL DEPARTMENT MEMBERS
###########################################################
SELECT 'DEPARTMENT_MEMBERS' AS table_name, COUNT(*) AS row_count FROM department_members;
SELECT 
    dm.member_id,
    CONCAT(m.first_name, ' ', m.last_name) AS member_name,
    dm.department_id,
    d.department_name
FROM department_members dm
JOIN members m ON dm.member_id = m.member_id
JOIN departments d ON dm.department_id = d.department_id;

###########################################################
-- VIEW ALL BUREAU POSITIONS
###########################################################
SELECT 'BUREAU_POSITIONS' AS table_name, COUNT(*) AS row_count FROM bureau_positions;
SELECT * FROM bureau_positions;

###########################################################
-- VIEW ALL BUREAU MEMBERS
###########################################################
SELECT 'BUREAU_MEMBERS' AS table_name, COUNT(*) AS row_count FROM bureau_members;
SELECT 
    bm.member_id,
    CONCAT(m.first_name, ' ', m.last_name) AS member_name,
    bm.position_id,
    bp.position_name,
    bp.position_alias,
    bm.start_term_date,
    bm.end_term_date
FROM bureau_members bm
JOIN members m ON bm.member_id = m.member_id
JOIN bureau_positions bp ON bm.position_id = bp.position_id;

###########################################################
-- VIEW ALL ACCOUNTS
###########################################################
SELECT 'ACCOUNTS' AS table_name, COUNT(*) AS row_count FROM accounts;
SELECT 
    account_id,
    username,
    email,
    is_active,
    member_id,
    creation_date,
    termination_date
FROM accounts;

###########################################################
-- VIEW ALL PICTURES
###########################################################
SELECT 'PICTURES' AS table_name, COUNT(*) AS row_count FROM pictures;
SELECT * FROM pictures;

###########################################################
-- VIEW ALL POSTS
###########################################################
SELECT 'POSTS' AS table_name, COUNT(*) AS row_count FROM posts;
SELECT * FROM posts;

###########################################################
-- VIEW ALL POSTS_PICTURES
###########################################################
SELECT 'POSTS_PICTURES' AS table_name, COUNT(*) AS row_count FROM posts_pictures;
SELECT 
    pp.picture_id,
    pp.post_id,
    p.post_author,
    pic.location
FROM posts_pictures pp
JOIN posts p ON pp.post_id = p.post_id
JOIN pictures pic ON pp.picture_id = pic.picture_id;

###########################################################
-- VIEW ALL ACCOUNT REQUESTS
###########################################################
SELECT 'ACCOUNT_REQUESTS' AS table_name, COUNT(*) AS row_count FROM account_requests;
SELECT * FROM account_requests;

###########################################################
-- SUMMARY: ALL TABLES ROW COUNTS
###########################################################
SELECT 'members' AS table_name, COUNT(*) AS row_count FROM members
UNION ALL
SELECT 'departments', COUNT(*) FROM departments
UNION ALL
SELECT 'department_members', COUNT(*) FROM department_members
UNION ALL
SELECT 'bureau_positions', COUNT(*) FROM bureau_positions
UNION ALL
SELECT 'bureau_members', COUNT(*) FROM bureau_members
UNION ALL
SELECT 'accounts', COUNT(*) FROM accounts
UNION ALL
SELECT 'pictures', COUNT(*) FROM pictures
UNION ALL
SELECT 'posts', COUNT(*) FROM posts
UNION ALL
SELECT 'posts_pictures', COUNT(*) FROM posts_pictures
UNION ALL
SELECT 'account_requests', COUNT(*) FROM account_requests;
*/