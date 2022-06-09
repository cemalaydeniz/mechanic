CREATE TABLE `Service`(
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Details` TEXT(65535),
    `Fee` DECIMAL(17,2) NOT NULL,
    `EnterDate` DATE NOT NULL,
    `ExitDate` DATE,
    `Vehicle_Id` INT NOT NULL,
    PRIMARY KEY(`Id`),
    FOREIGN KEY(`Vehicle_Id`) REFERENCES `Vehicle`(`Id`) ON DELETE CASCADE
)