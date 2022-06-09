CREATE TABLE `Vehicle`(
    `Id` INT NOT NULL AUTO_INCREMENT,
    `LicensePlate` VARCHAR(15) NOT NULL,
    `Make` VARCHAR(50) NOT NULL,
    `Model` VARCHAR(50) NOT NULL,
    `Year` INT NOT NULL,
    `Color` VARCHAR(30) NOT NULL,
    `Customer_Id` INT,
    PRIMARY KEY(`Id`),
    UNIQUE(`LicensePlate`),
    FOREIGN KEY(`Customer_Id`) REFERENCES `Customer`(`Id`) ON DELETE SET NULL
)