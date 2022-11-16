using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RPN_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RPNController : ControllerBase
    {
        private static readonly char[] Operands = new[]
        {
            '+', '/','*','-'
        };

        public static List<Rpn_stack> StackList { get; set; } = new();
        private static int counter;

        [HttpGet("op")]
        public IActionResult Get()
        {
            return Ok(Operands);
        }

        [HttpPost("stack")]
        public IActionResult CreateNewStack()
        {
            StackList.Add(new Rpn_stack
            {
                StackId = counter++,
                Stack = new Stack<int>()
            });
            return Ok();
        }

        [Route("~/op/{op}/stack/{stack_Id}")]
        [HttpPost]
        public IActionResult ApplyOperandToStack(string op, int stack_Id)
        {
            if (IsOperator(op))
            {
                var currentTask = StackList.Find(s => s.StackId == stack_Id);
                if (currentTask == null)
                {
                    return NotFound("stack_Id not found");
                }
                else
                {
                    if (currentTask.Stack.Count > 1)
                    {
                        try
                        {
                            Stack<int> rev = Reverse(currentTask.Stack);
                            int numTopOfStack = rev.Pop();
                            switch (op)
                            {
                                case "+":
                                    numTopOfStack += rev.Pop();
                                    break;

                                case "-":
                                    numTopOfStack -= rev.Pop();
                                    break;

                                case "*":
                                    numTopOfStack *= rev.Pop();
                                    break;

                                case "/":
                                    numTopOfStack /= rev.Pop();
                                    break;
                            }
                            rev.Push(numTopOfStack);
                            currentTask.Stack = Reverse(rev);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, ex);
                        }
                    }
                    return StatusCode(500, "Need at least two numbers in a stack to perform this operation");
                }
            }
            else
            {
                return NotFound(string.Format("Operand not found, please choose one on the list : {0}", string.Join(",", Operands)));
            }
        }

        [HttpGet("stack")]
        public IActionResult GetAllStacks()
        {
            return Ok(StackList);
        }

        [HttpGet("stack/{stack_Id}")]
        public IActionResult GetStack(int stack_Id)
        {
            var result = StackList.Find(s => s.StackId == stack_Id);
            return result == null ? NotFound("stack_Id not found") : Ok(result);
        }

        [HttpPost("stack/{stack_Id}/{value}")]
        public IActionResult PushValueToStack(int stack_Id, int value)
        {
            // j'ai pas cette route dans le besoin , je ne sais pas si c'est un oubli ou c'est moi qui n'a pas compris le besoin  ??
            // en gros je pousse une value dans une stack_Id pour la remplir 
            var currentStack = StackList.Find(s => s.StackId == stack_Id);
            if (currentStack == null)
            {
                return NotFound("stack_Id not found");
            }

            var reversedStack = Reverse(currentStack.Stack);
            reversedStack.Push(value);
            currentStack.Stack = Reverse(reversedStack);
            return Ok(currentStack);
        }

        [HttpDelete("{stack_Id:int}")]
        public IActionResult DeleteStack(int stack_Id)
        {
            var currentStack = StackList.Find(s => s.StackId == stack_Id);
            if (currentStack == null)
            {
                return NotFound("stack_Id not found");
            }
            StackList.Remove(currentStack);
            return Ok();
        }

        private static bool IsOperator(string op)
        {
            op = op.Replace("%2F", "/");
            return char.TryParse(op, out char value) && (value == '-' || value == '+' || value == '*' || value == '/');
        }

        private static Stack<int> Reverse(Stack<int> stack)
        {
            Stack<int> tempStack = new();
            while (stack.Count > 0)
            {
                tempStack.Push(stack.Pop());
            }
            return tempStack;
        }
    }
}