﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public partial class ProductDetailsPage : ComponentBase
    {

        [Parameter]
        public Guid Id { get; set; }

    }
}
