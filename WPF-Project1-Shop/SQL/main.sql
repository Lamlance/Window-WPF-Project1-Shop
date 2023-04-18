DROP TABLE product_category;
DROP TABLE categories;
DROP TABLE order_items;
DROP TABLE products;
DROP TABLE orders;
DROP TABLE customers;

CREATE TABLE categories (
    id SERIAL8 NOT NULL PRIMARY KEY,
    category_name VARCHAR(50) NOT NULL,
    UNIQUE(category_name)
);

CREATE TABLE products (
    id SERIAL8 NOT NULL PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL,
    descriptions TEXT,
    image_path TEXT NOT NULL,
    price FLOAT8 NOT NULL,
    numbers INT NOT NULL DEFAULT 0,
    created_at DATE NOT NULL,
    updated_at DATE,
    UNIQUE (product_name)
);


CREATE TABLE product_category (
    product_id BIGINT,
    category_id BIGINT,
    PRIMARY KEY (product_id, category_id)
);


CREATE TABLE order_items (
    id SERIAL8 NOT NULL PRIMARY KEY,
    product_id BIGINT,
    order_id BIGINT,
    price FLOAT8,
    quantity INT,
    created_at DATE NOT NULL,
    updated_at DATE
);


CREATE TABLE orders (
    id SERIAL8 NOT NULL PRIMARY KEY,
    customer_id BIGINT,
    subTotal FLOAT8 NOT NULL,
    created_at DATE NOT NULL,
    updated_at DATE,
    ship_address TEXT,
    status VARCHAR(10)
);

CREATE TABLE customers (
    id SERIAL8 NOT NULL PRIMARY KEY,
    phone VARCHAR(10) NOT NULL,
    first_name VARCHAR(10) NOT NULL,
    last_name VARCHAR(10) NOT NULL,
    middle_name VARCHAR(10),
    email VARCHAR(50),
    address TEXT
);


ALTER TABLE product_category
ADD CONSTRAINT FK_PC_PRODUCT
FOREIGN KEY (product_id)
REFERENCES products (id);

ALTER TABLE product_category
ADD CONSTRAINT FK_PC_CATEGORY
FOREIGN KEY (category_id)
REFERENCES categories (id);

ALTER TABLE order_items
ADD CONSTRAINT FK_OI_PRODUCT
FOREIGN KEY (product_id)
REFERENCES products (id);

ALTER TABLE order_items
ADD CONSTRAINT FK_OI_ORDER
FOREIGN KEY (order_id)
REFERENCES orders (id);

ALTER TABLE orders
ADD CONSTRAINT FK_ORDER_CUSTOMER
FOREIGN KEY (customer_id)
REFERENCES customers (id);