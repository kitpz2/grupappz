CREATE DATABASE `ass8_base`;
USE `ass8_base`;

BEGIN;
CREATE TABLE `files_plik` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `konto_id` integer NOT NULL,
    `sciezka` varchar(255) NOT NULL,
    `dataDodania` datetime NOT NULL,
    `prawaDostepu` integer NOT NULL,
    `wielkosc` integer NOT NULL,
    `hashValue` varchar(50) NOT NULL
)
;
ALTER TABLE `files_plik` ADD CONSTRAINT konto_id_refs_id_34c8aee5 FOREIGN KEY (`konto_id`) REFERENCES `accounts_konto` (`id`);
COMMIT;

BEGIN;
CREATE TABLE `accounts_konto` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `user_id` integer NOT NULL,
    `miasto` varchar(50) NOT NULL,
    `zainteresowania` longtext NOT NULL,
    `plec` varchar(1) NOT NULL
)
;
ALTER TABLE `accounts_konto` ADD CONSTRAINT user_id_refs_id_48ab1d40 FOREIGN KEY (`user_id`) REFERENCES `auth_user` (`id`);
COMMIT;

BEGIN;
CREATE TABLE `friends_userlink` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `from_user_id` integer NOT NULL,
    `to_user_id` integer NOT NULL,
    `date_added` datetime NOT NULL,
    UNIQUE (`to_user_id`, `from_user_id`)
)
;
ALTER TABLE `friends_userlink` ADD CONSTRAINT from_user_id_refs_id_140545d8 FOREIGN KEY (`from_user_id`) REFERENCES `accounts_konto` (`id`);
ALTER TABLE `friends_userlink` ADD CONSTRAINT to_user_id_refs_id_140545d8 FOREIGN KEY (`to_user_id`) REFERENCES `accounts_konto` (`id`);
COMMIT;

BEGIN;
CREATE TABLE `django_admin_log` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `action_time` datetime NOT NULL,
    `user_id` integer NOT NULL,
    `content_type_id` integer NULL,
    `object_id` longtext NULL,
    `object_repr` varchar(200) NOT NULL,
    `action_flag` smallint UNSIGNED NOT NULL,
    `change_message` longtext NOT NULL
)
;
ALTER TABLE `django_admin_log` ADD CONSTRAINT content_type_id_refs_id_288599e6 FOREIGN KEY (`content_type_id`) REFERENCES `django_content_type` (`id`);
ALTER TABLE `django_admin_log` ADD CONSTRAINT user_id_refs_id_c8665aa FOREIGN KEY (`user_id`) REFERENCES `auth_user` (`id`);
COMMIT;

BEGIN;
CREATE TABLE `auth_permission` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `name` varchar(50) NOT NULL,
    `content_type_id` integer NOT NULL,
    `codename` varchar(100) NOT NULL,
    UNIQUE (`content_type_id`, `codename`)
)
;
ALTER TABLE `auth_permission` ADD CONSTRAINT content_type_id_refs_id_728de91f FOREIGN KEY (`content_type_id`) REFERENCES `django_content_type` (`id`);
CREATE TABLE `auth_group` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `name` varchar(80) NOT NULL UNIQUE
)
;
CREATE TABLE `auth_user` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `username` varchar(30) NOT NULL UNIQUE,
    `first_name` varchar(30) NOT NULL,
    `last_name` varchar(30) NOT NULL,
    `email` varchar(75) NOT NULL,
    `password` varchar(128) NOT NULL,
    `is_staff` bool NOT NULL,
    `is_active` bool NOT NULL,
    `is_superuser` bool NOT NULL,
    `last_login` datetime NOT NULL,
    `date_joined` datetime NOT NULL
)
;
CREATE TABLE `auth_message` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `user_id` integer NOT NULL,
    `message` longtext NOT NULL
)
;
ALTER TABLE `auth_message` ADD CONSTRAINT user_id_refs_id_650f49a6 FOREIGN KEY (`user_id`) REFERENCES `auth_user` (`id`);
CREATE TABLE `auth_group_permissions` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `group_id` integer NOT NULL,
    `permission_id` integer NOT NULL,
    UNIQUE (`group_id`, `permission_id`)
)
;
ALTER TABLE `auth_group_permissions` ADD CONSTRAINT group_id_refs_id_3cea63fe FOREIGN KEY (`group_id`) REFERENCES `auth_group` (`id`);
ALTER TABLE `auth_group_permissions` ADD CONSTRAINT permission_id_refs_id_5886d21f FOREIGN KEY (`permission_id`) REFERENCES `auth_permission` (`id`);
CREATE TABLE `auth_user_groups` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `user_id` integer NOT NULL,
    `group_id` integer NOT NULL,
    UNIQUE (`user_id`, `group_id`)
)
;
ALTER TABLE `auth_user_groups` ADD CONSTRAINT user_id_refs_id_7ceef80f FOREIGN KEY (`user_id`) REFERENCES `auth_user` (`id`);
ALTER TABLE `auth_user_groups` ADD CONSTRAINT group_id_refs_id_f116770 FOREIGN KEY (`group_id`) REFERENCES `auth_group` (`id`);
CREATE TABLE `auth_user_user_permissions` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `user_id` integer NOT NULL,
    `permission_id` integer NOT NULL,
    UNIQUE (`user_id`, `permission_id`)
)
;
ALTER TABLE `auth_user_user_permissions` ADD CONSTRAINT user_id_refs_id_dfbab7d FOREIGN KEY (`user_id`) REFERENCES `auth_user` (`id`);
ALTER TABLE `auth_user_user_permissions` ADD CONSTRAINT permission_id_refs_id_67e79cb FOREIGN KEY (`permission_id`) REFERENCES `auth_permission` (`id`);
COMMIT;

BEGIN;
CREATE TABLE `django_content_type` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `name` varchar(100) NOT NULL,
    `app_label` varchar(100) NOT NULL,
    `model` varchar(100) NOT NULL,
    UNIQUE (`app_label`, `model`)
)
;
COMMIT;

BEGIN;
CREATE TABLE `django_session` (
    `session_key` varchar(40) NOT NULL PRIMARY KEY,
    `session_data` longtext NOT NULL,
    `expire_date` datetime NOT NULL
)
;
COMMIT;

BEGIN;
CREATE TABLE `django_site` (
    `id` integer AUTO_INCREMENT NOT NULL PRIMARY KEY,
    `domain` varchar(100) NOT NULL,
    `name` varchar(50) NOT NULL
)
;
COMMIT;
