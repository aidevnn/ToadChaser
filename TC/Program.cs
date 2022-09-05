using TC;


// var sg = TableOps.CreateHeader("a");
// var sg = TableOps.CreateHeader("b");
var sg = TableOps.CreateHeader("a", "cbc-1");

// var rels = TableOps.CreateHeader("a3", "b3", "abab");
// var rels = TableOps.CreateHeader("a4", "b3", "abab");
// var rels = TableOps.CreateHeader("a4", "b2", "abab");
var rels = TableOps.CreateHeader("a3", "b2", "c5", "abc");
// var rels = TableOps.CreateHeader("a2", "b3", "c5", "abc");
// var rels = TableOps.CreateHeader("a4", "a2b-2", "b-1aba");
// var rels = TableOps.CreateHeader("a2", "b3", "c4", "ab = ba", "bc = cb", "acacac");
// var rels = TableOps.CreateHeader("a2", "b8", "a2b-2", "aba-1b");
// var rels = TableOps.CreateHeader("a5", "b4", "ab=ba");
// var rels = TableOps.CreateHeader("a2", "b2", "c2", "d2", "ababab", "bcbcbc", "cdcdcd", "acac", "adad", "bdbd"); // S5

// var sg = TableOps.CreateHeader("y");
// var rels = TableOps.CreateHeader("y", "b2", "c5", "cbc-1"); // TO DO
// var rels = TableOps.CreateHeader("y", "a5", "b4", "c2", "ab=ba", "acac", "bcbc");
// var rels = TableOps.CreateHeader("y", "a4", "b3", "abab");
// var rels = TableOps.CreateHeader("y", "a5", "b4", "abababab", "a2ba-1b-1"); // F20
// var rels = TableOps.CreateHeader("y", "a2", "b2", "c2", "bcbcbc", "acacac", "abab"); // S4
// var rels = TableOps.CreateHeader("y", "a2", "b2", "c2", "d2", "ababab", "bcbcbc", "cdcdcd", "acac", "adad", "bdbd"); // S5

var tOps = new TableOps(sg, rels);
tOps.ToddCoxeter();
