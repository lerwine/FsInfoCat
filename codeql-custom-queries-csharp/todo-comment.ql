/**
 * @name TODO Comments
 * @kind problem
 * @problem.severity recommendation
 * @id csharp/fsinfocat/todo-comment
 * @description Finds comments containing the word "TODO".
 */

import csharp

from CommentLine c
where c.getText().regexpMatch("(?si).*\\bTODO\\b.*")
select c, c.getText()
