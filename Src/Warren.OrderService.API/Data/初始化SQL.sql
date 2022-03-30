CREATE DATABASE orderservice_0;

USE orderservice_0;

CREATE TABLE `order_info_0` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_1` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_2` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_3` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_0` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_1` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_2` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_3` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE DATABASE orderservice_1;

USE orderservice_1;

CREATE TABLE `order_info_0` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_1` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_2` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_info_3` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_0` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_1` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_2` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `order_index_shipper_3` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  PRIMARY KEY (`order_id`) USING BTREE,
  KEY `shipper_number` (`shipper_number`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE DATABASE orderservice_2022_0;
CREATE DATABASE orderservice_2022_1;
CREATE DATABASE orderservice_2022_2;
CREATE DATABASE orderservice_2022_3;

USE orderservice_2022_0;

CREATE TABLE `order_info` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

USE orderservice_2022_1;

CREATE TABLE `order_info` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

USE orderservice_2022_2;

CREATE TABLE `order_info` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

USE orderservice_2022_3;

CREATE TABLE `order_info` (
  `order_id` bigint unsigned NOT NULL COMMENT '订单id',
  `shipper_number` varchar(50) NOT NULL COMMENT '运单号',
  `forecast_date` datetime NOT NULL COMMENT '预报日期',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;