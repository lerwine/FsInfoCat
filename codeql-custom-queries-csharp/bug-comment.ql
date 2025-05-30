/**
 * @name BUG Comments
 * @kind problem
 * @problem.severity error
 * @id csharp/fsinfocat/bug-comment
 * @description Finds comments containing the word "BUG".
 */

import csharp

from CommentLine c
where c.getText().regexpMatch("(?si).*\\bBUG\\b.*")
select c, c.getText()
