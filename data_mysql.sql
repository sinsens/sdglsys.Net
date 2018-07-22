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

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for t_bill
-- ----------------------------
DROP TABLE IF EXISTS `t_bill`;
CREATE TABLE `t_bill` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '账单ID',
  `pid` int(11) DEFAULT NULL COMMENT '用量登记ID',
  `quota_id` int(11) DEFAULT NULL COMMENT '基础配额ID',
  `rates_id` int(11) DEFAULT NULL COMMENT '费率ID',
  `dorm_id` int(11) DEFAULT NULL COMMENT '园区ID',
  `building_id` int(11) DEFAULT NULL COMMENT '宿舍楼ID',
  `room_id` int(11) DEFAULT NULL COMMENT '宿舍ID',
  `cold_water_cost` decimal(8,2) DEFAULT NULL COMMENT '冷水费用',
  `hot_water_cost` decimal(8,2) DEFAULT NULL COMMENT '热水费用',
  `electric_cost` decimal(8,2) DEFAULT NULL COMMENT '电费',
  `note` varchar(255) DEFAULT NULL COMMENT '备注',
  `post_date` datetime DEFAULT NULL COMMENT '生成时间',
  `mod_date` datetime DEFAULT NULL COMMENT '修改时间',
  `is_active` smallint(6) DEFAULT NULL COMMENT '状态',
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_building
-- ----------------------------
DROP TABLE IF EXISTS `t_building`;
CREATE TABLE `t_building` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pid` int(11) DEFAULT NULL,
  `vid` varchar(15) DEFAULT NULL,
  `nickname` varchar(20) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pid` (`pid`),
  CONSTRAINT `t_building_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_dorm
-- ----------------------------
DROP TABLE IF EXISTS `t_dorm`;
CREATE TABLE `t_dorm` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nickname` varchar(20) DEFAULT NULL,
  `type` tinyint(1) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`type` in (0,1)),
  CONSTRAINT `CONSTRAINT_2` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_log
-- ----------------------------
DROP TABLE IF EXISTS `t_log`;
CREATE TABLE `t_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login_name` varchar(15) DEFAULT NULL,
  `ip` varchar(20) DEFAULT NULL,
  `info` text DEFAULT NULL,
  `log_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_notice
-- ----------------------------
DROP TABLE IF EXISTS `t_notice`;
CREATE TABLE `t_notice` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login_name` varchar(15) DEFAULT NULL,
  `title` varchar(40) DEFAULT NULL,
  `content` text DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  `mod_date` datetime DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `login_name` (`login_name`),
  CONSTRAINT `t_notice_ibfk_1` FOREIGN KEY (`login_name`) REFERENCES `t_user` (`login_name`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_quota
-- ----------------------------
DROP TABLE IF EXISTS `t_quota`;
CREATE TABLE `t_quota` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hot_water_value` float DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  `post_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_rate
-- ----------------------------
DROP TABLE IF EXISTS `t_rate`;
CREATE TABLE `t_rate` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `post_date` datetime DEFAULT NULL,
  `hot_water_value` float DEFAULT NULL,
  `cold_water_value` float DEFAULT NULL,
  `electric_value` float DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_room
-- ----------------------------
DROP TABLE IF EXISTS `t_room`;
CREATE TABLE `t_room` (
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
  CONSTRAINT `t_room_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_room_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_user
-- ----------------------------
DROP TABLE IF EXISTS `t_user`;
CREATE TABLE `t_user` (
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
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_used
-- ----------------------------
DROP TABLE IF EXISTS `t_used`;
CREATE TABLE `t_used` (
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
  CONSTRAINT `t_used_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_ibfk_4` FOREIGN KEY (`post_uid`) REFERENCES `t_user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`is_active` in (0,1))
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_used_total
-- ----------------------------
DROP TABLE IF EXISTS `t_used_total`;
CREATE TABLE `t_used_total` (
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
  CONSTRAINT `t_used_total_ibfk_1` FOREIGN KEY (`pid`) REFERENCES `t_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_total_ibfk_2` FOREIGN KEY (`dorm_id`) REFERENCES `t_dorm` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_used_total_ibfk_3` FOREIGN KEY (`building_id`) REFERENCES `t_building` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
