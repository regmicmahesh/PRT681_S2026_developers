import fs from "node:fs/promises";

// 使用Promise风格的函数读取文件内容
fs.readFile("example.txt", "utf8")
  .then((data) => {
    console.log(data);
  })
  .catch((error) => {
    console.error(error);
  });
