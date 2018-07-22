/*
Navicat MariaDB Data Transfer

Source Server         : localhost_3306
Source Server Version : 100302
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MariaDB
Target Server Version : 100302
File Encoding         : 65001

Date: 2018-06-30 11:52:48
*/

-- SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for t_dorm
-- ----------------------------
DROP TABLE IF EXISTS `t_dorm`;
CREATE TABLE `t_dorm` (
  `id` identity(1,1) PRIMARY KEY,
  `nickname` varchar(20),
  `type` tinyint(1),
  `note` varchar(255),
  `is_active` tinyint(1),
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`type` in (0,1)),
  CONSTRAINT `CONSTRAINT_2` CHECK (`is_active` in (0,1))
);


-- ----------------------------
-- Table structure for t_building
-- ----------------------------
DROP TABLE IF EXISTS `t_building`;
CREATE TABLE `t_building` (
  `id` identity(1,1) PRIMARY KEY,
  `pid` int(11),
  `vid` varchar(15),
  `nickname` varchar(20),
  `note` varchar(255),
  `is_active` tinyint(1),
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  CONSTRAINT `t_building_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_user
-- ----------------------------
DROP TABLE IF EXISTS `t_user`;
CREATE TABLE `t_user` (
  `id` identity(1,1) PRIMARY KEY,
  `login_name` varchar(15) NOT NULL,
  `nickname` varchar(15),
  `pwd` varchar(64),
  `note` varchar(255),
  `phone` varchar(11),
  `pid` int(11),
  `role` int(11),
  `reg_date` DATE,
  `is_active` INT(1),
  PRIMARY KEY (`id`,`login_name`),
  UNIQUE KEY `login_name` (`login_name`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_log
-- ----------------------------
DROP TABLE IF EXISTS `t_log`;
CREATE TABLE `t_log` (
  `id` identity(1,1) PRIMARY KEY,
  `login_name` varchar(15) DEFAULT '',
  `ip` varchar(20),
  `info` text,
  `log_date` DATE,
  PRIMARY KEY (`id`)
) -- ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_notice
-- ----------------------------
DROP TABLE IF EXISTS `t_notice`;
CREATE TABLE `t_notice` (
  `id` identity(1,1) PRIMARY KEY,
  `login_name` varchar(15),
  `title` varchar(40),
  `content` TEXT,
  `post_date` datetime,
  `mod_date` datetime,
  `is_active` bit(1),
  CONSTRAINT `t_notice_ibfk_1` FOREIGN KEY (`login_name`) REFERENCES `t_user` (`login_name`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_quota
-- ----------------------------
DROP TABLE IF EXISTS `t_quota`;
CREATE TABLE `t_quota` (
  `id` identity(1,1) PRIMARY KEY,
  `hot_water_value` float,
  `cold_water_value` float,
  `electric_value` float,
  `note` varchar(255),
  `is_active` tinyint(1),
  `post_date` datetime,
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_rate
-- ----------------------------
DROP TABLE IF EXISTS `t_rate`;
CREATE TABLE `t_rate` (
  `id` identity(1,1) PRIMARY KEY,
  `post_date` datetime,
  `hot_water_value` float,
  `cold_water_value` float,
  `electric_value` float,
  `note` varchar(255),
  `is_active` tinyint(1),
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_room
-- ----------------------------
DROP TABLE IF EXISTS `t_room`;
CREATE TABLE `t_room` (
  `id` identity(1,1) PRIMARY KEY,
  `pid` int(11),
  `dorm_id` int(11),
  `vid` varchar(15),
  `nickname` varchar(20),
  `note` varchar(255),
  `is_active` tinyint(1),
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  CONSTRAINT `t_room_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_room_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_used
-- ----------------------------
DROP TABLE IF EXISTS `t_used`;
CREATE TABLE `t_used` (
  `id` identity(1,1) PRIMARY KEY,
  `pid` int(11),
  `dorm_id` int(11),
  `building_id` int(11),
  `post_date` datetime,
  `post_uid` int(11),
  `cold_water_value` float,
  `hot_water_value` float,
  `electric_value` float,
  `note` varchar(255),
  `is_active` tinyint(1),
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  KEY `post_uid` (`post_uid`),
  CONSTRAINT `t_used_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_4` FOREIGN KEY (`post_uid`) REFERENCES `t_user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
);

-- ----------------------------
-- Table structure for t_used_total
-- ----------------------------
DROP TABLE IF EXISTS `t_used_total`;
CREATE TABLE `t_used_total` (
  `id` identity(1,1) PRIMARY KEY,
  `pid` int(11),
  `dorm_id` int(11),
  `building_id` int(11),
  `post_date` datetime,
  `cold_water_value` float,
  `hot_water_value` float,
  `electric_value` float,
  `note` varchar(255),
  PRIMARY KEY (`id`),
  UNIQUE KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  CONSTRAINT `t_used_total_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_total_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_total_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);


-- ----------------------------
-- Table structure for t_bill
-- ----------------------------
DROP TABLE IF EXISTS `t_bill`;
CREATE TABLE `t_bill` (
  `id` identity(1,1) COMMENT '账单ID',
  `pid` int(11) COMMENT '用量登记ID',
  `quota_id` int(11),
  `rates_id` int(11),
  `dorm_id` int(11),
  `building_id` int(11),
  `room_id` int(11),
  `cold_water_cost` decimal(8,2),
  `hot_water_cost` decimal(8,2),
  `electric_cost` decimal(8,2),
  `note` varchar(255),
  `post_date` datetime,
  `mod_date` datetime,
  `is_active` smallint(6),
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `rates_id` (`rates_id`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  KEY `room_id` (`room_id`),
  CONSTRAINT `t_bill_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_used` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_bill_ibfk_2` FOREIGN KEY (`rates_id`) REFERENCES `t_rate` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_bill_ibfk_3` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_bill_ibfk_4` FOREIGN KEY (`building_id`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_bill_ibfk_5` FOREIGN KEY (`room_id`) REFERENCES `t_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);