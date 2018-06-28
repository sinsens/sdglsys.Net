/*
Navicat MariaDB Data Transfer

Source Server         : localhost_3306
Source Server Version : 100302
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MariaDB
Target Server Version : 100302
File Encoding         : 65001

Date: 2018-06-28 20:35:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for bill
-- ----------------------------
DROP TABLE IF EXISTS `bill`;
CREATE TABLE `bill` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '账单ID',
  `pid` int(11) DEFAULT NULL COMMENT '用量登记ID',
  `quota_id` int(11) DEFAULT NULL,
  `rates_id` int(11) DEFAULT NULL,
  `dorm_id` int(11) DEFAULT NULL,
  `building_id` int(11) DEFAULT NULL,
  `room_id` int(11) DEFAULT NULL,
  `cold_water_cost` decimal(8,2) DEFAULT NULL,
  `hot_water_cost` decimal(8,2) DEFAULT NULL,
  `electric_cost` decimal(8,2) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  `mod_date` datetime DEFAULT NULL,
  `is_active` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `rates_id` (`rates_id`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  KEY `room_id` (`room_id`),
  CONSTRAINT `bill_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `used` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `bill_ibfk_2` FOREIGN KEY (`rates_id`) REFERENCES `rates` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `bill_ibfk_3` FOREIGN KEY (`dorm_id`) REFERENCES `dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `bill_ibfk_4` FOREIGN KEY (`building_id`) REFERENCES `building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `bill_ibfk_5` FOREIGN KEY (`room_id`) REFERENCES `room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for building
-- ----------------------------
DROP TABLE IF EXISTS `building`;
CREATE TABLE `building` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `vid` varchar(15) DEFAULT NULL,
  `nickname` varchar(20) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  CONSTRAINT `building_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dorm
-- ----------------------------
DROP TABLE IF EXISTS `dorm`;
CREATE TABLE `dorm` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nickname` varchar(20) DEFAULT NULL,
  `type` tinyint(1) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`type` in (0,1)),
  CONSTRAINT `CONSTRAINT_2` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for log
-- ----------------------------
DROP TABLE IF EXISTS `log`;
CREATE TABLE `log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login_name` varchar(15) DEFAULT NULL,
  `ip` varchar(20) DEFAULT NULL,
  `info` text DEFAULT NULL,
  `log_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for notice
-- ----------------------------
DROP TABLE IF EXISTS `notice`;
CREATE TABLE `notice` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login_name` varchar(15) DEFAULT NULL,
  `title` varchar(40) DEFAULT NULL,
  `content` text DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  `mod_date` datetime DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `login_name` (`login_name`),
  CONSTRAINT `notice_ibfk_1` FOREIGN KEY (`login_name`) REFERENCES `users` (`login_name`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for quota
-- ----------------------------
DROP TABLE IF EXISTS `quota`;
CREATE TABLE `quota` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hot_water_value` float DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for rates
-- ----------------------------
DROP TABLE IF EXISTS `rates`;
CREATE TABLE `rates` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `post_date` datetime DEFAULT NULL,
  `hot_water_value` float DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for room
-- ----------------------------
DROP TABLE IF EXISTS `room`;
CREATE TABLE `room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `dorm_id` int(11) DEFAULT NULL,
  `vid` varchar(15) DEFAULT NULL,
  `nickname` varchar(20) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  CONSTRAINT `room_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `room_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for used
-- ----------------------------
DROP TABLE IF EXISTS `used`;
CREATE TABLE `used` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `dorm_id` int(11) DEFAULT NULL,
  `building_id` int(11) DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  `post_uid` int(11) DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `hot_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  KEY `post_uid` (`post_uid`),
  CONSTRAINT `used_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `used_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `used_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `used_ibfk_4` FOREIGN KEY (`post_uid`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for used_total
-- ----------------------------
DROP TABLE IF EXISTS `used_total`;
CREATE TABLE `used_total` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `dorm_id` int(11) DEFAULT NULL,
  `building_id` int(11) DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `hot_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `pid` (`pid`),
  KEY `dorm_id` (`dorm_id`),
  KEY `building_id` (`building_id`),
  CONSTRAINT `used_total_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `used_total_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `used_total_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login_name` varchar(15) NOT NULL,
  `nickname` varchar(15) DEFAULT NULL,
  `pwd` varchar(64) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `phone` varchar(11) DEFAULT NULL,
  `pid` int(11) DEFAULT NULL,
  `role` int(11) DEFAULT NULL,
  `reg_date` datetime DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`,`login_name`),
  UNIQUE KEY `login_name` (`login_name`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
