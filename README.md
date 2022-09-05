# ToadChaser
Todd Coxeter Algorithm in CSharp for generating Cosets classes for a Subgroup H of a finitely presented group G by generators and relators.

By convention x^-1 is denoted X, uppercase character means the invert.
Example :
```
var sg = TableOps.CreateHeader("a");
var rels = TableOps.CreateHeader("a3", "b3", "abab");

var tOps = new TableOps(sg, rels);
tOps.ToddCoxeter();
```
will output
```
#### Step 1 Op : (1)·a=(1) ####
# SubGroup table
  a
┌─┬─┐
│1│1│

# Relators table
  a a a b b b a b a b
┌─┬───┬─┬───┬─┬─────┬─┐
│1│1 1│1│? ?│1│1 ? ?│1│

# Classes table
  │ a│ A│
──┼──┼──┤
 1│ 1│ 1│


#### Step 2 Op : (1)·b=(2) ####
# SubGroup table
  a
┌─┬─┐
│1│1│

# Relators table
  a a a b b b a b a b
┌─┬───┬─┬───┬─┬─────┬─┐
│1│1 1│1│2 ?│1│1 2 ?│1│
│2│? ?│2│? 1│2│? 1 1│2│

# Classes table
  │ a│ b│ A│ B│
──┼──┼──┼──┼──┤
 1│ 1│ 2│ 1│  │
 2│  │  │  │ 1│


#### Step 3 Op : (2)·b=(3) ####
# SubGroup table
  a
┌─┬─┐
│1│1│

# Relators table
  a a a b b b a b a b
┌─┬───┬─┬───┬─┬─────┬─┐
│1│1 1│1│2 3│1│1 2 3│1│
│2│3 ?│2│3 1│2│3 1 1│2│
│3│? 2│3│1 2│3│? ? 2│3│

# Classes table
  │ a│ b│ A│ B│
──┼──┼──┼──┼──┤
 1│ 1│ 2│ 1│ 3│
 2│ 3│ 3│  │ 1│
 3│  │ 1│ 2│ 2│


#### Step 4 Op : (3)·a=(4) ####
# SubGroup table
  a
┌─┬─┐
│1│1│

# Relators table
  a a a b b b a b a b
┌─┬───┬─┬───┬─┬─────┬─┐
│1│1 1│1│2 3│1│1 2 3│1│
│2│3 4│2│3 1│2│3 1 1│2│
│3│4 2│3│1 2│3│4 4 2│3│
│4│2 3│4│4 4│4│2 3 4│4│

# Classes table
  │ a│ b│ A│ B│
──┼──┼──┼──┼──┤
 1│ 1│ 2│ 1│ 3│
 2│ 3│ 3│ 4│ 1│
 3│ 4│ 1│ 2│ 2│
 4│ 2│ 4│ 3│ 4│


####     End    ####
```
and
```
var sg = TableOps.CreateHeader("a", "cbc-1");
var rels = TableOps.CreateHeader("a3", "b2", "c5", "abc");

var tOps = new TableOps(sg, rels);
tOps.ToddCoxeter();
```
will output
```
#### Step 1 Op : (1)·a=(1) ####
# SubGroup table
  a c b C
┌─┬─┬───┬─┐
│1│1│? ?│1│

# Relators table
  b b a a a a b c c c c c c
┌─┬─┬─┬───┬─┬───┬─┬───────┬─┐
│1│?│1│1 1│1│1 ?│1│? ? ? ?│1│

# Classes table
  │ a│ A│
──┼──┼──┤
 1│ 1│ 1│


#### Step 2 Op : (1)·c=(2) ####
# SubGroup table
  a c b C
┌─┬─┬───┬─┐
│1│1│2 2│1│

# Relators table
  b b a a a a b c c c c c c
┌─┬─┬─┬───┬─┬───┬─┬───────┬─┐
│1│?│1│1 1│1│1 ?│1│2 ? ? ?│1│
│2│2│2│? ?│2│? 1│2│? ? ? 1│2│

# Classes table
  │ a│ b│ c│ A│ B│ C│
──┼──┼──┼──┼──┼──┼──┤
 1│ 1│  │ 2│ 1│  │  │
 2│  │ 2│  │  │ 2│ 1│


#### Step 3 Op : (1)·b=(3) ####
# SubGroup table
  a c b C
┌─┬─┬───┬─┐
│1│1│2 2│1│

# Relators table
  b b a a a a b c c c c c c
┌─┬─┬─┬───┬─┬───┬─┬───────┬─┐
│1│3│1│1 1│1│1 3│1│2 ? ? 3│1│
│2│2│2│3 ?│2│3 1│2│? ? 3 1│2│
│3│1│3│? 2│3│? ?│3│1 2 ? ?│3│

# Classes table
  │ a│ b│ c│ A│ B│ C│
──┼──┼──┼──┼──┼──┼──┤
 1│ 1│ 3│ 2│ 1│ 3│ 3│
 2│ 3│ 2│  │  │ 2│ 1│
 3│  │ 1│ 1│ 2│ 1│  │


#### Step 4 Op : (2)·c=(4) ####
# SubGroup table
  a c b C
┌─┬─┬───┬─┐
│1│1│2 2│1│

# Relators table
  b b a a a a b c c c c c c
┌─┬─┬─┬───┬─┬───┬─┬───────┬─┐
│1│3│1│1 1│1│1 3│1│2 4 ? 3│1│
│2│2│2│3 4│2│3 1│2│4 ? 3 1│2│
│3│1│3│4 2│3│4 ?│3│1 2 4 ?│3│
│4│?│4│2 3│4│2 2│4│? 3 1 2│4│

# Classes table
  │ a│ b│ c│ A│ B│ C│
──┼──┼──┼──┼──┼──┼──┤
 1│ 1│ 3│ 2│ 1│ 3│ 3│
 2│ 3│ 2│ 4│ 4│ 2│ 1│
 3│ 4│ 1│ 1│ 2│ 1│  │
 4│ 2│  │  │ 3│  │ 2│


#### Step 5 Op : (4)·c=(5) ####
# SubGroup table
  a c b C
┌─┬─┬───┬─┐
│1│1│2 2│1│

# Relators table
  b b a a a a b c c c c c c
┌─┬─┬─┬───┬─┬───┬─┬───────┬─┐
│1│3│1│1 1│1│1 3│1│2 4 5 3│1│
│2│2│2│3 4│2│3 1│2│4 5 3 1│2│
│3│1│3│4 2│3│4 5│3│1 2 4 5│3│
│4│5│4│2 3│4│2 2│4│5 3 1 2│4│
│5│4│5│5 5│5│5 4│5│3 1 2 4│5│

# Classes table
  │ a│ b│ c│ A│ B│ C│
──┼──┼──┼──┼──┼──┼──┤
 1│ 1│ 3│ 2│ 1│ 3│ 3│
 2│ 3│ 2│ 4│ 4│ 2│ 1│
 3│ 4│ 1│ 1│ 2│ 1│ 5│
 4│ 2│ 5│ 5│ 3│ 5│ 2│
 5│ 5│ 4│ 3│ 5│ 4│ 4│


####     End    ####
```