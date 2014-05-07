/*
Navicat MySQL Data Transfer

Source Server         : Sanguo
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sanguo

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-07 08:30:09
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `a_account`
-- ----------------------------
DROP TABLE IF EXISTS `a_account`;
CREATE TABLE `a_account` (
  `AccountID` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(20) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `Password` varchar(20) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `PlayerID` int(11) NOT NULL DEFAULT '0',
  `SessionKey` varchar(100) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `UpdateDate` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AccountID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=big5;

-- ----------------------------
-- Records of a_account
-- ----------------------------
INSERT INTO `a_account` VALUES ('2', 'dandan', 'silveran', '1', 'SessionKey:dandan', '2014-05-06 21:56:32');
