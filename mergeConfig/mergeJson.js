//合并文件夹中所有的json
const fs = require('fs');
const path = require('path');

//打包目录
const myFile = './src';
//目标目录
const outFile = './out'
//指定文件格式
const strFormat = '.json';

let newFile = {};
let result = [];
function getAllDoc(mypath = '.') {
  let items = fs.readdirSync(mypath);
  items.map(item => {
    let temp = path.join(mypath, item);
    if (fs.statSync(temp).isDirectory()) {
      //递归遍历文件夹
      getAllDoc(temp)
      result = result.concat(getAllDoc(temp));
    } else {
      let p = path.extname(temp);
      if (p.toLowerCase() === strFormat) {
        result.push(temp);
      }
    }
  })
  return result;
}

var file = getAllDoc(myFile);
function readJson(fileName) {
  let bin = fs.readFileSync(fileName);
  let str = bin.toString();
  str = str ? JSON.parse(str) : str;
  return str;
}
console.log('准备合并以下json文件')
console.log(file)
for (let i = 0; i < file.length; i++) {
  let foo = file[i];
  if (foo) {
    let key = path.basename(foo);
    key = key.replace(strFormat, '')
    newFile[key] = readJson(foo);
  }
}

fs.writeFile(outFile + '/config.json', JSON.stringify(newFile), (err) => {
  if (err) {
    console.log('错误:', err);
    return;
  }
  console.log('写入成功');
})