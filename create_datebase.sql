DROP SCHEMA IF EXISTS `v_foody`;
CREATE SCHEMA `v_foody`;

CREATE TABLE v_foody.commission_config (
	id INT auto_increment NOT NULL,
	commission_rate FLOAT DEFAULT 0 NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT commission_config_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE v_foody.`role` (
	id int auto_increment NOT NULL,
	name varchar(50) NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT role_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.building (
	id INT auto_increment NOT NULL,
	name nvarchar(200) NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT building_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE v_foody.distance (
	id INT auto_increment NOT NULL,
	distance FLOAT DEFAULT 0 NOT NULL,
	building_id_from INT NOT NULL,
	building_id_to INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT distance_pk PRIMARY KEY (id),
	CONSTRAINT distance_from_building_FK FOREIGN KEY (building_id_from) REFERENCES v_foody.building(id),
	CONSTRAINT distance_to_building_FK FOREIGN KEY (building_id_to) REFERENCES v_foody.building(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.account (
	id INT auto_increment NOT NULL,
	phone_number varchar(20) NOT NULL,
	password varchar(250) NOT NULL,
	avatar_url varchar(300) NULL,
	first_name nvarchar(150) NOT NULL,
	last_name nvarchar(150) NOT NULL,
	email varchar(200) NOT NULL,
	status INT NOT NULL,
	role_id INT NOT NULL,
	building_id INT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT account_pk PRIMARY KEY (id),
	CONSTRAINT account_phone_number_unique UNIQUE KEY (phone_number),
	CONSTRAINT account_email_unique UNIQUE KEY (email),
	CONSTRAINT account_role_FK FOREIGN KEY (role_id) REFERENCES v_foody.`role`(id),
	CONSTRAINT account_building_FK FOREIGN KEY (building_id) REFERENCES v_foody.building(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.notification (
	id INT auto_increment NOT NULL,
	title nvarchar(200) NOT NULL,
	content nvarchar(400) NOT NULL,
	readed BIT DEFAULT 0 NOT NULL,
	account_id INT NOT NULL,
	role_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT notification_pk PRIMARY KEY (id),
	CONSTRAINT notification_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id),
	CONSTRAINT notification_role_FK FOREIGN KEY (role_id) REFERENCES v_foody.`role`(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE v_foody.contribution_type (
	id INT auto_increment NOT NULL,
	title nvarchar(200) NOT NULL,
	description nvarchar(400) NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT contribution_type_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.contribution (
	id INT auto_increment NOT NULL,
	title nvarchar(200) NOT NULL,
	content nvarchar(400) NOT NULL,
	contribution_type_id INT NOT NULL,
	account_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT contribution_pk PRIMARY KEY (id),
	CONSTRAINT contribution_contribution_type_FK FOREIGN KEY (contribution_type_id) REFERENCES v_foody.contribution_type(id),
	CONSTRAINT contribution_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.shop (
	id INT auto_increment NOT NULL,
	name nvarchar(200) NOT NULL,
	logo_url varchar(300) NULL,
	banner_url varchar(300) NULL,
	description nvarchar(400) NULL,
	balance FLOAT DEFAULT 0 NOT NULL,
	phone_number varchar(20) NOT NULL,
	active_from DATETIME NOT NULL,
	active_to DATETIME NOT NULL,
	active BIT DEFAULT 0 NOT NULL,
	total_order INT DEFAULT 0 NOT NULL,
	total_product INT DEFAULT 0 NOT NULL,
	total_rating INT DEFAULT 0 NOT NULL,
	status INT NOT NULL,
	minimum_value_order_freeship FLOAT DEFAULT 0 NOT NULL,
	shipping_fee FLOAT DEFAULT 0 NOT NULL,
	building_id INT NOT NULL,
	account_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT shop_pk PRIMARY KEY (id),
	CONSTRAINT shop_phone_number_unique UNIQUE KEY (phone_number),
	CONSTRAINT shop_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id),
	CONSTRAINT shop_building_FK FOREIGN KEY (building_id) REFERENCES v_foody.building(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.favourite_shop (
	id INT auto_increment NOT NULL,
	shop_id INT NOT NULL,
	account_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT favourite_shop_pk PRIMARY KEY (id),
	CONSTRAINT favourite_shop_shop_FK FOREIGN KEY (shop_id) REFERENCES v_foody.shop(id),
	CONSTRAINT favourite_shop_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.category (
	id INT auto_increment NOT NULL,
	name nvarchar(200) NOT NULL,
	description nvarchar(400) NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT category_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.product (
	id INT auto_increment NOT NULL,
	name nvarchar(200) NOT NULL,
	description nvarchar(400) NOT NULL,
	price FLOAT DEFAULT 0 NOT NULL,
	image_url varchar(300) NOT NULL,
	total_order INT DEFAULT 0 NOT NULL,
	status INT NOT NULL,
	shop_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT product_pk PRIMARY KEY (id),
	CONSTRAINT product_shop_FK FOREIGN KEY (shop_id) REFERENCES v_foody.shop(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.product_category (
	category_id INT NOT NULL,
	product_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT product_category_pk PRIMARY KEY (category_id,product_id),
	CONSTRAINT product_category_product_FK FOREIGN KEY (product_id) REFERENCES v_foody.product(id),
	CONSTRAINT product_category_category_FK FOREIGN KEY (category_id) REFERENCES v_foody.category(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.question (
	id INT auto_increment NOT NULL,
	question_type INT NOT NULL,
	description nvarchar(300) NOT NULL,
	status INT NOT NULL,
	product_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT question_pk PRIMARY KEY (id),
	CONSTRAINT question_product_FK FOREIGN KEY (product_id) REFERENCES v_foody.product(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.`option` (
	id INT auto_increment NOT NULL,	
	description nvarchar(300) NOT NULL,
	is_pricing BIT DEFAULT 0 NOT NULL,
	price FLOAT DEFAULT 0 NOT NULL,
	status INT NOT NULL,
	question_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT Option_pk PRIMARY KEY (id),
	CONSTRAINT option_question_FK FOREIGN KEY (question_id) REFERENCES v_foody.question(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.shop_promotion (
	id INT auto_increment NOT NULL,
	amount_rate FLOAT DEFAULT 0 NOT NULL,
	minimum_order_value FLOAT DEFAULT 0 NOT NULL,
	maximum_apply_value FLOAT DEFAULT 0 NOT NULL,
	amount_value FLOAT DEFAULT 0 NOT NULL,
	apply_type INT NOT NULL,
	start_date DATETIME NOT NULL,
	end_date DATETIME NOT NULL,
	usage_limit INT DEFAULT 0 NOT NULL,
	used INT DEFAULT 0 NOT NULL,
	shop_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT shop_promotion_pk PRIMARY KEY (id),
	CONSTRAINT shop_promotion_shop_FK FOREIGN KEY (shop_id) REFERENCES v_foody.shop(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE v_foody.platform_promotion (
	id INT auto_increment NOT NULL,
	amount_rate FLOAT DEFAULT 0 NOT NULL,
	minimum_order_value FLOAT DEFAULT 0 NOT NULL,
	maximum_apply_value FLOAT DEFAULT 0 NOT NULL,
	amount_value FLOAT DEFAULT 0 NOT NULL,
	apply_type INT NOT NULL,
	start_date DATETIME NOT NULL,
	end_date DATETIME NOT NULL,
	usage_limit INT DEFAULT 0 NOT NULL,
	number_of_used INT DEFAULT 0 NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT platform_promotion_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.person_promotion (
	id INT auto_increment NOT NULL,
	amount_rate FLOAT DEFAULT 0 NOT NULL,
	minimum_order_value FLOAT DEFAULT 0 NOT NULL,
	maximum_apply_value FLOAT DEFAULT 0 NOT NULL,
	amount_value FLOAT DEFAULT 0 NOT NULL,
	apply_type INT NOT NULL,
	start_date DATETIME NOT NULL,
	end_date DATETIME NOT NULL,
	usage_limit INT DEFAULT 0 NOT NULL,
	number_of_used INT DEFAULT 0 NOT NULL,
	account_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT person_promotion_pk PRIMARY KEY (id),
	CONSTRAINT person_promotion_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.`transaction` (
	id INT auto_increment NOT NULL,
	amount FLOAT DEFAULT 0 NOT NULL,
	status INT NOT NULL,
	transaction_type INT NOT NULL,
	payment_thirdparty_id varchar(200) NULL,
	payment_thirdparty_content varchar(200) NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT transaction_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.`order` (
	id INT auto_increment NOT NULL,
	status INT NOT NULL,
	shipping_fee FLOAT DEFAULT 0 NOT NULL,
	note nvarchar(300) NULL,
	total_price FLOAT DEFAULT 0 NOT NULL,
	total_promotion FLOAT DEFAULT 0 NOT NULL,
	transaction_id INT NOT NULL,
	shop_promotion_id INT NULL,
	platform_promotion_id INT NULL,
	personal_promotion_id INT NULL,
	shop_id INT NOT NULL,
	account_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT order_pk PRIMARY KEY (id),
	CONSTRAINT order_transaction_FK FOREIGN KEY (transaction_id) REFERENCES v_foody.`transaction`(id),
	CONSTRAINT order_shop_promotion_FK FOREIGN KEY (shop_promotion_id) REFERENCES v_foody.shop_promotion(id),
	CONSTRAINT order_platform_promotion_FK FOREIGN KEY (platform_promotion_id) REFERENCES v_foody.platform_promotion(id),
	CONSTRAINT order_person_promotion_FK FOREIGN KEY (personal_promotion_id) REFERENCES v_foody.person_promotion(id),
	CONSTRAINT order_shop_FK FOREIGN KEY (shop_id) REFERENCES v_foody.shop(id),
	CONSTRAINT order_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.order_detail (
	id INT auto_increment NOT NULL,
	quantity INT DEFAULT 0 NOT NULL,
	price FLOAT DEFAULT 0 NOT NULL,
	product_id INT NOT NULL,
	order_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT order_detail_pk PRIMARY KEY (id),
	CONSTRAINT order_detail_order_FK FOREIGN KEY (order_id) REFERENCES v_foody.`order`(id),
	CONSTRAINT order_detail_product_FK FOREIGN KEY (product_id) REFERENCES v_foody.product(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE v_foody.order_detail_option (
	order_detail_id INT NOT NULL,
	option_id INT NOT NULL,
	price FLOAT DEFAULT 0 NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT order_detail_option_pk PRIMARY KEY (order_detail_id,option_id),
	CONSTRAINT order_detail_option_order_detail_fk FOREIGN KEY (order_detail_id) REFERENCES v_foody.order_detail(id),
	CONSTRAINT order_detail_option_option_fk FOREIGN KEY (option_id) REFERENCES v_foody.`option`(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE v_foody.feedback (
	id INT auto_increment NOT NULL,
	rating INT NOT NULL,
	comment nvarchar(300) NULL,
	account_id INT NOT NULL,
	order_id INT NOT NULL,
	created_date DATETIME NOT NULL,
	updated_date DATETIME NOT NULL,
	CONSTRAINT feedback_pk PRIMARY KEY (id),
	CONSTRAINT feedback_account_FK FOREIGN KEY (account_id) REFERENCES v_foody.account(id),
	CONSTRAINT feedback_order_FK FOREIGN KEY (order_id) REFERENCES v_foody.`order`(id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO v_foody.`role` (name, created_date, updated_date)
VALUES ('Some Role', NOW(), NOW());

INSERT INTO v_foody.building (name, created_date, updated_date)
VALUES ('Some Building', NOW(), NOW());

INSERT INTO v_foody.building (name, created_date, updated_date)
VALUES ('Some Building', NOW(), NOW());

INSERT INTO v_foody.distance (distance, building_id_from, building_id_to, created_date, updated_date)
VALUES (10.5, 1, 2, NOW(), NOW());

INSERT INTO v_foody.account (phone_number, password, avatar_url, first_name, last_name, email, status, role_id, building_id, created_date, updated_date)
VALUES ('123456789', 'password123', 'https://example.com/avatar.jpg', 'John', 'Doe', 'john.doe@example.com', 1, 1, 1, NOW(), NOW());














