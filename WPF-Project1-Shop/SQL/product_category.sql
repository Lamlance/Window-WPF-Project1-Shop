drop table product_category;

CREATE TABLE product_category (
    product_id BIGINT,
    category_id BIGINT,
    PRIMARY KEY (product_id, category_id)
);

ALTER TABLE product_category
ADD CONSTRAINT FK_PC_PRODUCT
FOREIGN KEY (product_id)
REFERENCES products (id);

ALTER TABLE product_category
ADD CONSTRAINT FK_PC_CATEGORY
FOREIGN KEY (category_id)
REFERENCES categories (id);

insert into product_category (product_id, category_id) values (1, 5);
insert into product_category (product_id, category_id) values (2, 5);
insert into product_category (product_id, category_id) values (3, 5);
insert into product_category (product_id, category_id) values (4, 5);
insert into product_category (product_id, category_id) values (5, 5);
insert into product_category (product_id, category_id) values (6, 5);
insert into product_category (product_id, category_id) values (7, 5);
insert into product_category (product_id, category_id) values (8, 5);
insert into product_category (product_id, category_id) values (9, 5);
insert into product_category (product_id, category_id) values (10, 5);
insert into product_category (product_id, category_id) values (11, 5);
insert into product_category (product_id, category_id) values (12, 5);
insert into product_category (product_id, category_id) values (13, 5);
insert into product_category (product_id, category_id) values (14, 5);
insert into product_category (product_id, category_id) values (15, 5);
insert into product_category (product_id, category_id) values (16, 5);
insert into product_category (product_id, category_id) values (17, 5);
insert into product_category (product_id, category_id) values (18, 5);
insert into product_category (product_id, category_id) values (19, 5);
insert into product_category (product_id, category_id) values (20, 5);
insert into product_category (product_id, category_id) values (21, 5);
insert into product_category (product_id, category_id) values (22, 5);
insert into product_category (product_id, category_id) values (23, 5);
insert into product_category (product_id, category_id) values (24, 5);
insert into product_category (product_id, category_id) values (25, 5);
insert into product_category (product_id, category_id) values (26, 5);
insert into product_category (product_id, category_id) values (27, 5);
insert into product_category (product_id, category_id) values (28, 5);
insert into product_category (product_id, category_id) values (29, 5);
insert into product_category (product_id, category_id) values (30, 5);
